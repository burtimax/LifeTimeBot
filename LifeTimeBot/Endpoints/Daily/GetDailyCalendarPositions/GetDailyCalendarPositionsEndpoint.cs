using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;

namespace LifeTimeBot.Endpoints.Daily.GetDailyCalendarPositions;

sealed class GetDailyCalendarPositionsRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public DateTime GetMaxDate() => EndDate >= StartDate ? EndDate : StartDate;
    public DateTime GetMinDate() => StartDate <= EndDate ? StartDate : EndDate;
}

sealed class GetDailyCalendarPositionsResponse 
{
    public List<DateContainsActivitiesModel> Data { get; set; }
}

sealed class DateContainsActivitiesModel 
{
    public DateTime Date { get; set; }
    public bool HasActivities { get; set; }
}


sealed class GetDailyCalendarPositionsEndpoint : Endpoint<GetDailyCalendarPositionsRequest, GetDailyCalendarPositionsResponse>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public GetDailyCalendarPositionsEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Get("/get-calendar-positions");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Получить список наличия позиций по дням";
            s.Description = $"Получить список наличия позиций по дням";
        });
    }

    public override async Task HandleAsync(GetDailyCalendarPositionsRequest r, CancellationToken c)
    {
        GetDailyCalendarPositionsResponse response = new();
        List<DateContainsActivitiesModel> data = new();
        response.Data = data;
        
        GetActivitiesDto dto = new()
        {
            Start = r.GetMinDate(),
            End = r.GetMaxDate(),
            Confirmed = true,
            CropDates = false,
            StrictRestrictions = false,
            BotIds = new List<long>() { r.BotId },
            ChatIds = new List<long>() { r.ChatId },
            Order = $"{nameof(ActivityEntity.StartTime)}"
        };
        
        var res = await _activityService.GetActivities(dto);
        if (res.TotalCount == 0)
        {
            await SendAsync(response);
            return;
        }
        
        var d = r.GetMinDate().Date;
        var max = r.GetMaxDate();

        while (d <= max)
        {
            data.Add(new()
            {
                Date = d,
                HasActivities = res.Data.Any(a => a.StartTime is not null && a.StartTime.Value.Date == d)
            });
            d = d.AddDays(1);
        }

        response.Data = data;
        await SendAsync(response);
    }
}