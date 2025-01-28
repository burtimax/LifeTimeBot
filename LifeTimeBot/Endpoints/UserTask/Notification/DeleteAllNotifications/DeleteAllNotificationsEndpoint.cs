using FastEndpoints;
using LifeTimeBot.Services.UserTask;
using LifeTimeBot.Services.UserTaskNotification;
using Microsoft.AspNetCore.Mvc;

namespace LifeTimeBot.Endpoints.UserTask.Notification.DeleteAllNotifications;

sealed class DeleteAllNotificationsRequest 
{
    public long Id { get; set; }
}

sealed class DeleteAllNotificationsEndpoint : Endpoint<DeleteAllNotificationsRequest>
{
    private readonly UserTaskNotificationService _notificationService;

    public DeleteAllNotificationsEndpoint(UserTaskNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override void Configure()
    {
        Delete("/delete/{Id}");
        AllowAnonymous();
        Group<UserTaskNotificationGroup>();
        Summary(s =>
        {
            s.Summary = "Удалить уведомление для задачи";
            s.Description = "Удалить уведомление для задачи";
        });
    }

    public override async Task HandleAsync(DeleteAllNotificationsRequest r, CancellationToken c)
    {
        await _notificationService.DeleteUserTaskNotificationAsync(r.Id);
        await SendNoContentAsync();
    }
} 