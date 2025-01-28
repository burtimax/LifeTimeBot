using FastEndpoints;
using LifeTimeBot.Db.AppDb.Entities.Notifications;
using LifeTimeBot.Services.UserTask;
using LifeTimeBot.Services.UserTaskNotification;
using LifeTimeBot.Services.UserTaskNotification.Dto;
using Microsoft.AspNetCore.Mvc;
using IMapper = MapsterMapper.IMapper;

namespace LifeTimeBot.Endpoints.UserTask.Notification.UpdateNotification;

sealed class UpdateUserTaskNotificationRequest 
{
    public long Id { get; set; }
    public DateTime? NotificationDateTime { get; set; }
    public bool? IsEnabled { get; set; }
}

sealed class UpdateUserTaskNotificationEndpoint : Endpoint<UpdateUserTaskNotificationRequest, UserTaskNotificationEntity?>
{
    private readonly UserTaskNotificationService _notificationService;
    private readonly IMapper _mapper;

    public UpdateUserTaskNotificationEndpoint(UserTaskNotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<UserTaskNotificationGroup>();
        Summary(s =>
        {
            s.Summary = "Обновить статус уведомления";
            s.Description = "Обновляет статус существующего уведомления";
        });
    }

    public override async Task HandleAsync(UpdateUserTaskNotificationRequest r, CancellationToken c)
    {
        UpdateUserTaskNotificationDto dto = _mapper.Map<UpdateUserTaskNotificationDto>(r);
        var notification = await _notificationService.UpdateUserTaskNotificationAsync(dto);
        await SendAsync(notification);
    }
}