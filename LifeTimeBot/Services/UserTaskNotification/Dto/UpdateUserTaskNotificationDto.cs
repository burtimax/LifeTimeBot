namespace LifeTimeBot.Services.UserTaskNotification.Dto;

public class UpdateUserTaskNotificationDto
{
    public long Id { get; set; }
    public DateTime? NotificationDateTime { get; set; }
    public int? Utc { get; set; }
    public bool? IsEnabled { get; set; }
}