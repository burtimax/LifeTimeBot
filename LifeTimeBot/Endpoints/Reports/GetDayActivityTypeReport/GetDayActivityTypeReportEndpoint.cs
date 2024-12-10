using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;

namespace LifeTimeBot.Endpoints.Reports.GetDayActivityTypeReport;

sealed class GetDayActivityTypeReportRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime DateTime { get; set; }
    
}

sealed class GetDayActivityTypeReportResponse
{
    public Dictionary<string, int> TypeScales { get; set; }
    public int ProductivityPercent { get; set; }
    public List<ActivityEntity> Data { get; set; }
}

/// <summary>
/// Дневной отчет по колесу баланса
/// </summary>
sealed class GetDayActivityTypeReportEndpoint : Endpoint<GetDayActivityTypeReportRequest, GetDayActivityTypeReportResponse>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;
    private readonly UserUtcService _userUtcService;

    public GetDayActivityTypeReportEndpoint(AppDbContext db, ActivityService activityService, UserUtcService userUtcService)
    {
        _db = db;
        _activityService = activityService;
        _userUtcService = userUtcService;
    }

    public override void Configure()
    {
        Get("/day-activity-type");
        AllowAnonymous();
        Group<ReportGroup>();
        Summary(s =>
        {
            s.Summary = "Получить отчет по типам активностей за день (гистограма)";
            s.Description = $"Получить отчет по типам активностей за день (гистограма)";
        });
    }

    public override async Task HandleAsync(GetDayActivityTypeReportRequest r, CancellationToken c)
    {
        GetActivitiesDto dto = new GetActivitiesDto()
        {
            BotIds = new List<long>() { r.BotId },
            ChatIds = new List<long>() { r.ChatId },
            Start = r.DateTime.Date,
            End = r.DateTime.Date.AddDays(1),
            Confirmed = true,
            CropDates = true,
        };
        
        GetDayActivityTypeReportResponse response = new GetDayActivityTypeReportResponse();
        
        List<ActivityEntity> activities = (await _activityService.GetActivities(dto, true))?.Data ?? new();
        response.Data = activities;

        response.TypeScales = CountTypeScales(activities);
        response.ProductivityPercent = CountProductivityPercent(activities);

        await SendAsync(response);
    }

    /// <summary>
    /// Возвращает высоту шкалы [0..100] для типа активностей.
    /// </summary>
    /// <param name="activities"></param>
    /// <returns></returns>
    private Dictionary<string, int> CountTypeScales(List<ActivityEntity> activities)
    {
        Dictionary<string, int> typeScales = new();
        typeScales.Add("0", 0);
        typeScales.Add("1", 0);
        typeScales.Add("2", 0);
        typeScales.Add("3", 0);
        typeScales.Add("4", 0);
        typeScales.Add("5", 0);
        
        var typeMinutes = activities.GroupBy(a => a.Type).Select(g => new
        {
            Type = ((int) g.Key).ToString(),
            TotalMinutes = g.Sum(a => a.TotalMinutes)
        });
        
        int max = typeMinutes.Max(a => a.TotalMinutes);

        if (max == 0) return typeScales;

        foreach (var typeMinute in typeMinutes)
        {
            typeScales[typeMinute.Type] = Convert.ToInt32(Math.Round((1.0 * typeMinute.TotalMinutes / max) * 100));
        }

        return typeScales;
    }
    
    /// <summary>
    /// Рассчитываем процент эффективности дневного дня.
    /// Сколько из времени дневного дня мы занимались полезными делами.
    /// </summary>
    /// <param name="activities"></param>
    /// <returns></returns>
    private int CountProductivityPercent(List<ActivityEntity> activities)
    {
        if (activities == null || activities.Count == 0) return 0;
        
        int usefullMinutes = activities
            .Where(a => a.Type != ActivityType.Relax 
                        && a.Type != ActivityType.Sleep
                        && a.Type != ActivityType.None)
            .Sum(a => a.TotalMinutes);
        
        int allMinutes = activities
            .Where(a => a.Type != ActivityType.Sleep)
            .Sum(a => a.TotalMinutes);

        if (allMinutes == 0) return 0;
        
        int productivityPercent = Convert.ToInt32(Math.Round(1.0 * usefullMinutes / allMinutes * 100));
        
        return productivityPercent;
    }
}