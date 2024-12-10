using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Endpoints.Daily.CreateDaily;

sealed class CreateDailyRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime StartTime { get; set; }
   
    public DateTime EndTime { get; set; }
   
    public string Description { get; set; }
    public string? Comment { get; set; }
    public ActivityType Type { get; set; }
    public List<BalanceType> BalanceTypes { get; set; }
   
}

sealed class CreateDailyEndpoint : Endpoint<CreateDailyRequest, ActivityEntity>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public CreateDailyEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Post("/activity");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Создать активность";
            s.Description = $"Создать активность";
        });
    }

    public override async Task HandleAsync(CreateDailyRequest r, CancellationToken c)
    {
        if (r.EndTime <= r.StartTime)
        {
            await SendAsync(null, 500);
            return;
        }
        
        ActivityEntity? activity = new ActivityEntity()
        {
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Description = r.Description,
            Comment = r.Comment,
            Type = r.Type,
            BalanceTypes = r.BalanceTypes,
            Confirmed = true,
            BotId = r.BotId,
            TelegramChatId = r.ChatId
        };

        _db.Activity.Add(activity);
        await _db.SaveChangesAsync();
        await SendAsync(activity);
    }
}