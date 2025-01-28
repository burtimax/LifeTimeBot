using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Db.AppDb.Entities.Notifications;
using LifeTimeBot.Services.UserTaskNotification.Dto;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Extensions;

namespace LifeTimeBot.Services.UserTaskNotification;

/// <summary>
/// Сервис для работы с уведомлениями задач пользователя
/// </summary>
public class UserTaskNotificationService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public UserTaskNotificationService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    /// <summary>
    /// Создать уведомление для задачи
    /// </summary>
    public async Task<UserTaskNotificationEntity> CreateNotificationAsync(CreateUserTaskNotificationDto dto)
    {
        var notification = new UserTaskNotificationEntity
        {
            Status = NotificationStatus.New,
        };

        _mapper.Map(dto, notification);
        
        _db.Add(notification);
        await _db.SaveChangesAsync();
        return notification;
    }

    /// <summary>
    /// Получить уведомления по фильтрам
    /// </summary>
    public async Task<List<UserTaskNotificationEntity>> GetUserTaskNotifications(GetUserTaskNotificationsDto dto)
    {
        return await _db.Set<UserTaskNotificationEntity>()
            .WhereIf(dto.BotId != null, u => u.BotId == dto.BotId)
            .WhereIf(dto.TelegramChatId != null, u => u.TelegramChatId == dto.TelegramChatId)
            .WhereIf(dto.Ids != null && dto.Ids.Any(), u => dto.Ids.Contains(u.Id))
            .OrderBy(n => n.NotificationDateTime)
            .ToListAsync();
    }
    
    /// <summary>
    /// Получить уведомления по идентификатору задачи
    /// </summary>
    public async Task<List<UserTaskNotificationEntity>> GetUserTaskNotificationsByTaskIdAsync(long taskId)
    {
        return await _db.Set<UserTaskNotificationEntity>()
            .Where(n => n.UserTaskId == taskId)
            .OrderBy(n => n.NotificationDateTime)
            .ToListAsync();
    }

    /// <summary>
    /// Получить активные уведомления для отправки
    /// </summary>
    public async Task<List<UserTaskNotificationEntity>> GetActiveUserTaskNotificationsForSendingAsync(DateTime currentTime)
    {
        return await _db.Set<UserTaskNotificationEntity>()
            .Where(n => n.Status == NotificationStatus.New 
                       && n.NotificationDateTime <= currentTime
                       && n.IsEnabled == true
                       && n.DeletedAt == null)
            .Include(n => n.UserTask)
            .OrderBy(n => n.NotificationDateTime)
            .ToListAsync();
    }

    /// <summary>
    /// Обновить статус уведомления
    /// </summary>
    public async Task<UserTaskNotificationEntity?> UpdateUserTaskNotificationAsync(UpdateUserTaskNotificationDto dto)
    {
        var notification = await _db.Set<UserTaskNotificationEntity>()
            .FirstOrDefaultAsync(n => n.Id == dto.Id);

        if (notification != null)
        {
            _mapper.Map(dto, notification);
            _db.UserTaskNotifications.Update(notification);
            await _db.SaveChangesAsync();
            return notification;
        }

        return null;
    }

    /// <summary>
    /// Удалить уведомление
    /// </summary>
    public async Task DeleteUserTaskNotificationAsync(long id)
    {
        var notification = await _db.Set<UserTaskNotificationEntity>()
            .FirstOrDefaultAsync(n => n.Id == id);

        if (notification != null)
        {
            _db.UserTaskNotifications.Remove(notification);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Удалить все уведомления задачи
    /// </summary>
    public async Task DeleteAllTaskNotificationsAsync(long taskId)
    {
        var notifications = await _db.Set<UserTaskNotificationEntity>()
            .Where(n => n.UserTaskId == taskId)
            .ToListAsync();

        if (notifications.Any())
        {
            _db.UserTaskNotifications.RemoveRange(notifications);
            await _db.SaveChangesAsync();
        }
    }
}