using FastEndpoints;

namespace LifeTimeBot.Endpoints.UserTask.Notification;

public sealed class UserTaskNotificationGroup : Group
{
    public UserTaskNotificationGroup()
    {
        Configure("user-task/notification", c =>
        {
            c.Tags("Уведомления задач");
        });
    }
} 