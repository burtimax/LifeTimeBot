using LifeTimeBot.Db.AppDb.Entities;

namespace LifeTimeBot.Services.UserTask.Dto;

public class GetTasksDto
{
    public long? BotId { get; set; }
    public long? TelegramChatId { get; set; }
    public List<long>? Ids { get; set; }
    public UserTaskType? Type { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}