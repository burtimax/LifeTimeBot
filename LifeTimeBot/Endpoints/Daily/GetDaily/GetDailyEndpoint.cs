using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;
using MultipleBotFrameworkEndpoints.Models;

namespace LifeTimeBot.Endpoints.Daily.GetDaily;

sealed class GetDailyRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public List<long>? Ids { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

sealed class GetDailyEndpoint : Endpoint<GetDailyRequest, PagedList<ActivityEntity>>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public GetDailyEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Get("/get");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Получить распорядок пользователя на определенный день";
            s.Description = $"Получить распорядок пользователя на определенный день";
        });
    }

    public override async Task HandleAsync(GetDailyRequest r, CancellationToken c)
    {
        var start = r.StartDate?.Date;
        var end = r.EndDate?.AddDays(1).Date;
        List<ActivityEntity> activities = new();

        GetActivitiesDto dto = new()
        {
            Start = start,
            End = end,
            Confirmed = true,
            CropDates = true,
            StrictRestrictions = false,
            BotIds = new List<long>() { r.BotId },
            ChatIds = new List<long>() { r.ChatId },
            Order = $"{nameof(ActivityEntity.StartTime)}"
        };

        if (r.Ids != null && r.Ids.Any())
        {
            dto.Ids = r.Ids;
        }
        
        var res = await _activityService.GetActivities(dto);
        await SendAsync(res);
    }
}