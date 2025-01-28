namespace LifeTimeBot.Db.AppDb.Entities.Notifications;

/// <summary>
/// Статус уведомления в системе
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// Статус не определен
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Новое уведомление
    /// </summary>
    New = 1,
    
    /// <summary>
    /// Уведомление в очереди на отправку
    /// </summary>
    InQueue = 2,
    
    /// <summary>
    /// Ошибка при отправке уведомления
    /// </summary>
    Failed = 3,
    
    /// <summary>
    /// Уведомление успешно отправлено
    /// </summary>
    Success = 4,
}