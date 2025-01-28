using System.ComponentModel;
using FastEndpoints;
using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services.UserTask;
using LifeTimeBot.Services.UserTask.Dto;
using MultipleBotFrameworkEndpoints.Models;
using IMapper = MapsterMapper.IMapper;

namespace LifeTimeBot.Endpoints.UserTask.GetTask;

sealed class GetTaskRequest
{
    [DefaultValue(AppConstants.DefaultBotId)]
    public long BotId { get; set; }
    [DefaultValue(AppConstants.DefaultChatId)]
    public long TelegramChatId { get; set; }
    public List<long>? Ids { get; set; }
    public UserTaskType? Type { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

sealed class GetTaskEndpoint : Endpoint<GetTaskRequest, List<UserTaskEntity>>
{
    private readonly UserTaskService _taskService;
    private readonly IMapper _mapper;

    public GetTaskEndpoint(UserTaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("/get");
        AllowAnonymous();
        Group<UserTaskGroup>();
        Summary(s =>
        {
            s.Summary = "Получить задачи пользователя";
            s.Description = "Получить задачи пользователя";
        });
    }

    public override async Task HandleAsync(GetTaskRequest r, CancellationToken c)
    {
        GetTasksDto dto = _mapper.Map<GetTasksDto>(r);
        var tasks = await _taskService.GetTasks(dto);

        await SendAsync(tasks);
    }
} 