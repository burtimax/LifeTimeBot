using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Db.AppDb.Entities.Notifications;

/// <summary>
/// Базовый класс для уведомлений.
/// </summary>
public class BaseNotification : BaseEntity<long>
{
    /// <summary>
    /// Статус уведомления
    /// </summary>
    [Comment("Статус уведомления")]
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Дата и время уведомления
    /// </summary>
    [Comment("Дата и время уведомления")]
    public DateTime NotificationDateTime { get; set; }

    /// <summary>
    /// Часовой пояс в формате UTC
    /// </summary>
    [Comment("Часовой пояс в формате UTC")]
    public int Utc { get; set; }
        
    /// <summary>
    /// Идентификатор бота
    /// </summary>
    [Comment("Идентификатор бота")]
    public long BotId { get; set; }

    /// <summary>
    /// Идентификатор чата в Telegram
    /// </summary>
    [Comment("Идентификатор чата в Telegram")]
    public long TelegramChatId { get; set; }
    
    /// <summary>
    /// Флаг активности уведомления
    /// </summary>
    [Comment("Флаг активности уведомления")]
    public bool IsEnabled { get; set; } = true;
}