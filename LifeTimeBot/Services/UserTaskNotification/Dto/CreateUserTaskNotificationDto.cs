namespace LifeTimeBot.Services.UserTaskNotification.Dto;

public class CreateUserTaskNotificationDto
{
    public long BotId { get; set; }
    public long TelegramChatId { get; set; }
    public long UserTaskId { get; set; }
    public DateTime NotificationDateTime { get; set; }
    public int Utc { get; set; }
}