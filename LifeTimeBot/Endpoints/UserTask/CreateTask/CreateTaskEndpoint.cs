using System.ComponentModel;
using FastEndpoints;
using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services.UserTask;

namespace LifeTimeBot.Endpoints.UserTask.CreateTask;

sealed class CreateTaskRequest 
{
    [DefaultValue(AppConstants.DefaultBotId)]
    public long BotId { get; set; }
    [DefaultValue(AppConstants.DefaultChatId)]
    public long ChatId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public UserTaskType Type { get; set; }
}

sealed class CreateTaskEndpoint : Endpoint<CreateTaskRequest, UserTaskEntity>
{
    private readonly UserTaskService _taskService;

    public CreateTaskEndpoint(UserTaskService taskService)
    {
        _taskService = taskService;
    }

    public override void Configure()
    {
        Post("/task");
        AllowAnonymous();
        Group<UserTaskGroup>();
        Summary(s =>
        {
            s.Summary = "Создать задачу";
            s.Description = "Создать новую задачу пользователя";
        });
    }

    public override async Task HandleAsync(CreateTaskRequest r, CancellationToken c)
    {
        var task = new UserTaskEntity
        {
            BotId = r.BotId,
            TelegramChatId = r.ChatId,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Description = r.Description,
            Comment = r.Comment,
            Type = r.Type,
            Confirmed = true
        };

        var result = await _taskService.CreateTaskAsync(task);
        await SendAsync(result);
    }
} 