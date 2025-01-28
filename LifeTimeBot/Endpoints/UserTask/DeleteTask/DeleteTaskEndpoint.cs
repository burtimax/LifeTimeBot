using FastEndpoints;
using LifeTimeBot.Services.UserTask;

namespace LifeTimeBot.Endpoints.UserTask.DeleteTask;

sealed class DeleteTaskRequest 
{
    public long Id { get; set; }
}

sealed class DeleteTaskEndpoint : Endpoint<DeleteTaskRequest>
{
    private readonly UserTaskService _taskService;

    public DeleteTaskEndpoint(UserTaskService taskService)
    {
        _taskService = taskService;
    }

    public override void Configure()
    {
        Delete("/task/{Id}");
        AllowAnonymous();
        Group<UserTaskGroup>();
        Summary(s =>
        {
            s.Summary = "Удалить задачу";
            s.Description = "Удалить существующую задачу";
        });
    }

    public override async Task HandleAsync(DeleteTaskRequest r, CancellationToken c)
    {
        var result = await _taskService.DeleteTaskAsync(r.Id);
        if (!result)
        {
            await SendErrorsAsync(StatusCodes.Status404NotFound);
            return;
        }
        
        await SendNoContentAsync();
    }
} 