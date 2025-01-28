using System.ComponentModel;
using FastEndpoints;
using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Db.AppDb.Entities.Notifications;
using LifeTimeBot.Services;
using LifeTimeBot.Services.UserTask;
using LifeTimeBot.Services.UserTaskNotification;
using LifeTimeBot.Services.UserTaskNotification.Dto;
using IMapper = MapsterMapper.IMapper;

namespace LifeTimeBot.Endpoints.UserTask.Notification.CreateNotification;

sealed class CreateNotificationRequest 
{
    public long UserTaskId { get; set; }
    public DateTime NotificationDateTime { get; set; }
}

sealed class CreateNotificationEndpoint : Endpoint<CreateNotificationRequest, UserTaskNotificationEntity>
{
    private readonly UserTaskService _taskService;
    private readonly UserTaskNotificationService _notificationService;
    private readonly UserUtcService _userUtcService;
    private readonly IMapper _mapper;

    public CreateNotificationEndpoint(UserTaskService taskService, UserTaskNotificationService notificationService, UserUtcService userUtcService, IMapper mapper)
    {
        _taskService = taskService;
        _notificationService = notificationService;
        _userUtcService = userUtcService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post("/create");
        AllowAnonymous();
        Group<UserTaskNotificationGroup>();
        Summary(s =>
        {
            s.Summary = "Создать уведомление для задачи";
            s.Description = "Создает новое уведомление для существующей задачи";
        });
    }

    public override async Task HandleAsync(CreateNotificationRequest r, CancellationToken c)
    {
        var task = await _taskService.GetTaskByIdAsync(r.UserTaskId);
        if (task == null)
        {
            await SendErrorsAsync(StatusCodes.Status404NotFound);
            return;
        }

        CreateUserTaskNotificationDto dto = _mapper.Map<CreateUserTaskNotificationDto>(r);
        dto.BotId = task.BotId;
        dto.TelegramChatId = task.TelegramChatId;
        dto.Utc = await _userUtcService.GetUserUtcOffset(task.BotId, task.TelegramChatId);
        
        var notification = await _notificationService.CreateNotificationAsync(dto);
        await SendAsync(notification);
    }
} 