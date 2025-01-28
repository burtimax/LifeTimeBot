using FastEndpoints;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services.UserTask;

namespace LifeTimeBot.Endpoints.UserTask.UpdateTask;

public sealed class UpdateTaskRequest 
{
    public long Id { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public UserTaskType? Type { get; set; }
}

sealed class UpdateTaskEndpoint : Endpoint<UpdateTaskRequest, UserTaskEntity?>
{
    private readonly UserTaskService _taskService;

    public UpdateTaskEndpoint(UserTaskService taskService)
    {
        _taskService = taskService;
    }

    public override void Configure()
    {
        Put("/task");
        AllowAnonymous();
        Group<UserTaskGroup>();
        Summary(s =>
        {
            s.Summary = "Обновить задачу";
            s.Description = "Обновить существующую задачу";
        });
    }

    public override async Task HandleAsync(UpdateTaskRequest r, CancellationToken c)
    {
        var existingTask = await _taskService.GetTaskByIdAsync(r.Id);
        if (existingTask == null)
        {
            await SendErrorsAsync(StatusCodes.Status404NotFound);
            return;
        }
        
        var result = await _taskService.UpdateTaskAsync(r);
        await SendAsync(result);
    }
} 