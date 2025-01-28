namespace LifeTimeBot.Services.UserTaskNotification.Dto;

public class GetUserTaskNotificationsDto
{
    public long? BotId { get; set; }
    public long? TelegramChatId { get; set; }
    public List<long>? Ids { get; set; }
}