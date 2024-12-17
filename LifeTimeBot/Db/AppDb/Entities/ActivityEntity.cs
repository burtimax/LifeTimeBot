using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Db.Entity;

namespace LifeTimeBot.Db.AppDb.Entities;

[Comment("Активности пользователей")]
public class ActivityEntity : BaseEntity<long>
{
    public long BotId { get; set; }
    [Comment("ИД чата, владельца активности")]
    public long TelegramChatId { get; set; }
    [Comment("Время начала активности")]
    public DateTimeOffset? StartTime { get; set; }
    [Comment("Время окончания активности")]
    public DateTimeOffset? EndTime { get; set; }
    [Comment("Описание активности")]
    public string? Description { get; set; }
    [Comment("Emoji активности")]
    public string? Emoji { get; set; }
    [Comment("Комментарий к активности, подробности от пользователя.")]
    public string? Comment { get; set; }
    [Comment("FileId голосового")]
    public string? AudioFileId { get; set; }
    [Comment("ИД сообщения")]
    public int? MessageId { get; set; }
    [Comment("Текст аудиосообщения")]
    public string? MessageText { get; set; }
    [Comment("Подтверждена активность пользователем? Правильно ли сформирована.")]
    public bool Confirmed { get; set; }
    [Comment("Тип активности.")]
    public ActivityType Type { get; set; }

    [Comment("Сферы жизненного баланса активности.")]
    public List<BalanceType> BalanceTypes { get; set; } = new();

    /// <summary>
    /// Длительность активности в минутах.
    /// </summary>
    [NotMapped]
    public int TotalMinutes => Convert.ToInt32(EndTime.HasValue ? EndTime.Value.Subtract(StartTime.Value).TotalMinutes : 0);
}