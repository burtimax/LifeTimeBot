using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace LifeTimeBot.Endpoints.Daily.DeleteDaily;

sealed class DeleteDailyActivityRequest 
{
    public long Id { get; set; }
   
}

sealed class DeleteDailyActivityEndpoint : Endpoint<DeleteDailyActivityRequest>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public DeleteDailyActivityEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Delete("/activity/{Id}");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Удалить активность";
            s.Description = $"Удалить активность";
        });
    }

    public override async Task HandleAsync(DeleteDailyActivityRequest r, CancellationToken c)
    {
        GetActivitiesDto dto = new()
        {
            Ids = new List<long>(){ r.Id }
        };
        var result = await _activityService.GetActivities(dto);
        var res = (result)?.Data?.FirstOrDefault();

        if (res == null)
        {
            await SendAsync(null);
            return;
        }
        
        _db.Entry(res).State = EntityState.Deleted;
        await _db.SaveChangesAsync();
        await SendAsync(null);
    }
}