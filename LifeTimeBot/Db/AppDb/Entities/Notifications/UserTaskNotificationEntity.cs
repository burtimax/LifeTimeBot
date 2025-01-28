using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Db.AppDb.Entities.Notifications;

/// <summary>
/// Сущность уведомления для пользовательской задачи
/// </summary>
public class UserTaskNotificationEntity : BaseNotification
{
    /// <summary>
    /// Идентификатор пользовательской задачи
    /// </summary>
    [Comment("Внешний ключ на задачу пользователя")]
    public long UserTaskId { get; set; }

    /// <summary>
    /// Связанная пользовательская задача
    /// </summary>
    [JsonIgnore]
    public UserTaskEntity UserTask { get; set; }
}