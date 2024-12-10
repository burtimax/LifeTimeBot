using System.Globalization;
using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Extensions;
using LifeTimeBot.Services;

namespace LifeTimeBot.Endpoints.Daily.GetDaysSequence;

sealed class GetDaysSequenceRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime? Date { get; set; }
    public int PageSize { get; set; } = 20;
    
    /// <summary>
    /// Запрашиваем предыдущий или следующий
    /// </summary>
    public bool? IsPrevious { get; set; }
    
    /// <summary>
    /// Выделяем сегодняшний день цветом.
    /// </summary>
    public DateTime? Today { get; set; }
}

sealed class DaysSequenceItem 
{
    public DateTime Date { get; set; }
    public string DayOfWeek { get; set; }
    public string Day { get; set; }
    public string Month { get; set; }
    public string Year { get; set; }
    public bool IsToday { get; set; }
}
sealed class GetDaysSequenceResponse 
{
    public List<DaysSequenceItem> Data { get; set; }
}

sealed class GetDaysSequenceEndpoint : Endpoint<GetDaysSequenceRequest, GetDaysSequenceResponse>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;
    private readonly UserUtcService _userUtcService;

    public GetDaysSequenceEndpoint(AppDbContext db, ActivityService activityService, UserUtcService userUtcService)
    {
        _db = db;
        _activityService = activityService;
        _userUtcService = userUtcService;
    }

    public override void Configure()
    {
        Get("/days-sequence");
        AllowAnonymous();
        Group<DailyGroup>();
        Summary(s =>
        {
            s.Summary = "Получить последовательность дней";
            s.Description = $"Получить последовательность дней";
        });
    }

    public override async Task HandleAsync(GetDaysSequenceRequest r, CancellationToken c)
    {
        DateTime date = GetDateFromRequest(r);
        DateTime today = await GetTodayFromRequest(r);
        DateTime start = r.IsPrevious == true ? date.Date.AddDays(-1 * r.PageSize - 1) : date.Date.AddDays(1);
        DateTime end = start.Date.AddDays(r.PageSize);

        GetDaysSequenceResponse response = new GetDaysSequenceResponse();
        response.Data = new List<DaysSequenceItem>();
        
        while (start <= end)
        {
            response.Data.Add(GetFromDay(start, today));
            start = start.AddDays(1);
        }

        await SendAsync(response);
    }

    private DateTime GetDateFromRequest(GetDaysSequenceRequest r)
    {
        return r.Date ?? DateTime.Now.Date.AddDays(-1 * (r.PageSize / 2));
    }
    
    private async Task<DateTime> GetTodayFromRequest(GetDaysSequenceRequest r)
    {
        int utcOffset = await _userUtcService.GetUserUtcOffset(r.BotId, r.ChatId);
        return r.Today ?? DateTime.Now.AddHours(-1 * utcOffset).Date;
    }

    private DaysSequenceItem GetFromDay(DateTime date, DateTime today)
    {
        return new()
        {
            Day = date.Day.ToString(),
            DayOfWeek = date.ToString("ddd", new CultureInfo("ru-RU")).FirstCharToUpper(),
            Month = date.ToString("MMMM", new CultureInfo("ru-RU")).FirstCharToUpper(),
            Year = date.ToString("yyyy", new CultureInfo("ru-RU")),
            IsToday = date.Date == today.Date,
            Date = date,
        };
    }
}