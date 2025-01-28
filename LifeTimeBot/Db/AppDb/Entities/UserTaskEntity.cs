using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LifeTimeBot.Db.AppDb.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Db.Entity;

namespace LifeTimeBot.Db.AppDb.Entities;

[Comment("Задачи пользователей")]
public class UserTaskEntity : BaseEntity<long>
{
    [Comment("ИД бота")]
    public long BotId { get; set; }
    [Comment("ИД чата, владельца активности")]
    public long TelegramChatId { get; set; }
    [Comment("Время начала задачи")]
    public DateTime? StartTime { get; set; }
    [Comment("Время окончания задачи")]
    public DateTime? EndTime { get; set; }
    [Comment("Описание задачи")]
    public string? Description { get; set; }
    [Comment("Комментарий к задаче, подробности от пользователя")]
    public string? Comment { get; set; }
    [Comment("Emoji задачи")]
    public string? Emoji { get; set; }
    [Comment("FileId голосового")]
    public string? AudioFileId { get; set; }
    [Comment("ИД сообщения")]
    public int? MessageId { get; set; }
    [Comment("Текст аудиосообщения")]
    public string? MessageText { get; set; }
    [Comment("Подтверждена задача пользователем? Правильно ли сформирована.")]
    public bool Confirmed { get; set; }
    [Comment("Тип задачи.")]
    public UserTaskType Type { get; set; }

    [JsonIgnore]
    public List<UserTaskNotificationEntity>? TaskNotifications { get; set; } = new();

    [NotMapped]
    public List<UserTaskNotificationEntity>? Notifications => TaskNotifications;
}