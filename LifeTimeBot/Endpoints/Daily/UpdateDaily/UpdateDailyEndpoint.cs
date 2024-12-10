using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Endpoints.Daily.UpdateDaily;

sealed class UpdateDailyRequest 
{
    public long Id { get; set; }
    
    public DateTime? StartTime { get; set; }
   
    public DateTime? EndTime { get; set; }
   
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public ActivityType? Type { get; set; }
    public List<BalanceType>? BalanceTypes { get; set; }
   
}

sealed class UpdateDailyEndpoint : Endpoint<UpdateDailyRequest, ActivityEntity>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public UpdateDailyEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Put("/activity");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Изменить данные активности";
            s.Description = $"Изменить данные";
        });
    }

    public override async Task HandleAsync(UpdateDailyRequest r, CancellationToken c)
    {
        GetActivitiesDto dto = new()
        {
            Ids = new List<long>(){ r.Id }
        };
        var res = (await _activityService.GetActivities(dto))?.Data?.FirstOrDefault();

        if (res == null)
        {
            await SendErrorsAsync(StatusCodes.Status404NotFound);
            return;
        }

        if (r.StartTime != null)
        {
            res.StartTime = r.StartTime.Value;
        }
        
        if (r.EndTime != null)
        {
            res.EndTime = r.EndTime.Value;
        }

        if (string.IsNullOrEmpty(r.Description) == false)
        {
            res.Description = r.Description;
        }
        
        if (string.IsNullOrEmpty(r.Comment) == false)
        {
            res.Comment = r.Comment;
        }
        
        if (r.Type is not null)
        {
            res.Type = r.Type.Value;
        }
        
        if (r.BalanceTypes is not null)
        {
            res.BalanceTypes = r.BalanceTypes;
        }
        
        _db.Entry(res).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        await SendAsync(res);
    }
}