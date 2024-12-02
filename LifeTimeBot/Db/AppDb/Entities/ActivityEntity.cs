using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Db.Entity;

namespace LifeTimeBot.Db.AppDb.Entities;

[Comment("Активности пользователей")]
public class ActivityEntity : BaseEntity<long>
{
    [Comment("ИД чата, владельца активности")]
    public long TelegramChatId { get; set; }
    [Comment("Время начала активности")]
    public DateTimeOffset? StartTime { get; set; }
    [Comment("Время окончания активности")]
    public DateTimeOffset? EndTime { get; set; }
    [Comment("Описание активности")]
    public string? Description { get; set; }
    [Comment("FileId голосового")]
    public string? AudioFileId { get; set; }
    [Comment("ИД сообщения")]
    public int? MessageId { get; set; }
    [Comment("Текст аудиосообщения")]
    public string? MessageText { get; set; }
    [Comment("Подтверждена активность пользователем? Правильно ли сформирована.")]
    public bool Confirmed { get; set; }
}