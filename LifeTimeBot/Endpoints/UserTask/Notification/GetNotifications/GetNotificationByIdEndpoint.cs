using System.ComponentModel;
using FastEndpoints;
using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities.Notifications;
using LifeTimeBot.Services.UserTask;
using LifeTimeBot.Services.UserTaskNotification;
using LifeTimeBot.Services.UserTaskNotification.Dto;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;

namespace LifeTimeBot.Endpoints.UserTask.Notification.GetNotifications;

sealed class GetNotificationsRequest
{
    [DefaultValue(AppConstants.DefaultBotId)]
    public long? BotId { get; set; }
    [DefaultValue(AppConstants.DefaultChatId)]
    public long? TelegramChatId { get; set; }
    public List<long>? Ids { get; set; }
}

sealed class GetNotificationByIdEndpoint : Endpoint<GetNotificationsRequest, List<UserTaskNotificationEntity>?>
{
    private readonly UserTaskNotificationService _notificationService;
    private readonly IMapper _mapper;

    public GetNotificationByIdEndpoint(UserTaskNotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/get");
        AllowAnonymous();
        Group<UserTaskNotificationGroup>();
        Summary(s =>
        {
            s.Summary = "Получить уведомление задачи по идентификатору";
            s.Description = "Получить уведомление задачи по идентификатору";
        });
    }

    public override async Task HandleAsync([FromQuery] GetNotificationsRequest r, CancellationToken c)
    {
        GetUserTaskNotificationsDto dto = _mapper.Map<GetUserTaskNotificationsDto>(r);
        var notifications = await _notificationService.GetUserTaskNotifications(dto);
        await SendAsync(notifications);
    }
}