using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Db.AppDb.Entities.Notifications;

/// <summary>
/// Базовый класс для уведомлений. Содержит основные поля, необходимые для работы с уведомлениями в системе.
/// </summary>
public class BaseNotification : BaseEntity<long>
{
    /// <summary>
    /// Статус уведомления. Определяет текущее состояние уведомления в системе (новое, в очереди, отправлено и т.д.)
    /// </summary>
    [Comment("Статус уведомления")]
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Дата и время уведомления. Определяет когда должно быть отправлено уведомление пользователю.
    /// </summary>
    [Comment("Дата и время уведомления")]
    public DateTime NotificationDateTime { get; set; }

    /// <summary>
    /// Часовой пояс в формате UTC. Используется для корректного отображения времени пользователю с учетом его временной зоны.
    /// </summary>
    [Comment("Часовой пояс в формате UTC")]
    public int Utc { get; set; }
        
    /// <summary>
    /// Идентификатор бота. Уникальный идентификатор бота, который будет отправлять уведомление.
    /// </summary>
    [Comment("Идентификатор бота")]
    public long BotId { get; set; }

    /// <summary>
    /// Идентификатор чата в Telegram. Уникальный идентификатор чата, в который будет отправлено уведомление.
    /// </summary>
    [Comment("Идентификатор чата в Telegram")]
    public long TelegramChatId { get; set; }
    
    /// <summary>
    /// Флаг активности уведомления. Определяет, активно ли уведомление и должно ли оно быть отправлено.
    /// </summary>
    [Comment("Флаг активности уведомления")]
    public bool IsEnabled { get; set; } = true;
}