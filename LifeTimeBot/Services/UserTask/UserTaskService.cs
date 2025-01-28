using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Endpoints.UserTask.UpdateTask;
using LifeTimeBot.Services.UserTask.Dto;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MultipleBotFrameworkEndpoints.Extensions;

namespace LifeTimeBot.Services.UserTask;

public class UserTaskService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserTaskService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // Create
    public async Task<UserTaskEntity> CreateTaskAsync(UserTaskEntity task)
    {
        await _dbContext.UserTasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    // Read
    public async Task<UserTaskEntity?> GetTaskByIdAsync(long id)
    {
        return await _dbContext.UserTasks.FindAsync(id);
    }

    public async Task<List<UserTaskEntity>> GetAllTasksAsync()
    {
        return await _dbContext.UserTasks.ToListAsync();
    }

    public async Task<List<UserTaskEntity>> GetTasksByUserAsync(long telegramChatId)
    {
        return await _dbContext.UserTasks
            .Include(t => t.TaskNotifications)
            .Where(t => t.TelegramChatId == telegramChatId)
            .ToListAsync();
    }
    
    /// <summary>
    /// Получает список задач пользователя с применением фильтров
    /// </summary>
    /// <param name="dto">Объект с параметрами фильтрации:
    /// - BotId: ID бота
    /// - ChatId: ID чата в Telegram
    /// - Ids: Список конкретных ID задач
    /// - Type: Тип задачи
    /// - StartTime: Начальная дата для фильтрации
    /// - EndTime: Конечная дата для фильтрации</param>
    /// <returns>Отфильтрованный список задач пользователя</returns>
    public async Task<List<UserTaskEntity>> GetTasks(GetTasksDto dto)
    {
        // Базовый запрос с фильтрацией по обязательным полям
        var query = _dbContext.UserTasks
            .Include(t => t.TaskNotifications)
            .WhereIf(dto.BotId != null, t => t.BotId == dto.BotId)
            .WhereIf(dto.TelegramChatId != null, t => t.TelegramChatId == dto.TelegramChatId)
            .WhereIf(dto.Type != null, t => t.Type == dto.Type)
            .WhereIf(dto.StartTime != null, t => t.StartTime >= dto.StartTime)
            .WhereIf(dto.EndTime != null, t => t.EndTime <= dto.EndTime)
            .WhereIf(dto.Ids != null && dto.Ids.Any(), t => dto.Ids.Contains(t.Id))
            .OrderBy(t => t.EndTime);

        return await query.ToListAsync();
    }

    public async Task<List<UserTaskEntity>> GetUserTasksByTypeAsync(long telegramChatId, UserTaskType type)
    {
        return await _dbContext.UserTasks
            .Include(t => t.TaskNotifications)
            .Where(t => t.TelegramChatId == telegramChatId && t.Type == type)
            .ToListAsync();
    }

    // Update
    public async Task<UserTaskEntity?> UpdateTaskAsync(UpdateTaskRequest task)
    {
        var existingTask = await _dbContext.UserTasks.FindAsync(task.Id);
        if (existingTask == null)
            return null;

        _mapper.Map(task, existingTask);
        await _dbContext.SaveChangesAsync();
        return existingTask;
    }

    // Delete
    public async Task<bool> DeleteTaskAsync(long id)
    {
        var task = await _dbContext.UserTasks.FindAsync(id);
        if (task == null)
            return false;

        _dbContext.UserTasks.Remove(task);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<int> DeleteUserTasksAsync(long telegramChatId)
    {
        var tasks = await _dbContext.UserTasks
            .Where(t => t.TelegramChatId == telegramChatId)
            .ToListAsync();

        _dbContext.UserTasks.RemoveRange(tasks);
        return await _dbContext.SaveChangesAsync();
    }
}