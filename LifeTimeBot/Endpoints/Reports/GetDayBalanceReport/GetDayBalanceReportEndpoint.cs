using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;

namespace LifeTimeBot.Endpoints.Reports.GetDayBalanceReport;

sealed class GetDayBalanceReportRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime DateTime { get; set; }
    
}

sealed class GetDayBalanceReportResponse
{
    public Dictionary<string, bool> Balances { get; set; }
    public List<ActivityEntity> Data { get; set; }
}

/// <summary>
/// Дневной отчет по колесу баланса
/// </summary>
sealed class GetDayBalanceReportEndpoint : Endpoint<GetDayBalanceReportRequest, GetDayBalanceReportResponse>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;

    public GetDayBalanceReportEndpoint(AppDbContext db, ActivityService activityService)
    {
        _db = db;
        _activityService = activityService;
    }

    public override void Configure()
    {
        Get("/day-balance");
        AllowAnonymous();
        Group<ReportGroup>();
        Summary(s =>
        {
            s.Summary = "Получить отчет по балансу за день";
            s.Description = $"Получить отчет по балансу за день";
        });
    }

    public override async Task HandleAsync(GetDayBalanceReportRequest r, CancellationToken c)
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
        
        GetDayBalanceReportResponse response = new GetDayBalanceReportResponse();
        
        List<ActivityEntity> activities = (await _activityService.GetActivities(dto, true))?.Data ?? new();
        response.Data = activities;
        
        response.Balances = new Dictionary<string, bool>();
        response.Balances.Add("0", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.None) ?? false));
        response.Balances.Add("1", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.СareerWorkBusiness) ?? false));
        response.Balances.Add("2", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.LoveFamilyChildren) ?? false));
        response.Balances.Add("3", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.HealthyAndSport) ?? false));
        response.Balances.Add("4", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.FriendsAndCommunity) ?? false));
        response.Balances.Add("5", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.HobbyAndBrightnessOfLife) ?? false));
        response.Balances.Add("6", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.PersonalDevelopmentAndEducation) ?? false));
        response.Balances.Add("7", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.Finance) ?? false));
        response.Balances.Add("8", activities.Any(a => a.BalanceTypes?.Contains(BalanceType.Spirituality) ?? false));

        await SendAsync(response);
    }
}