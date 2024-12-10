using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Resources;
using LifeTimeBot.Services;
using LifeTimeBot.Services.Dto;
using LifeTimeBot.Services.LLM;
using Microsoft.Extensions.Options;

namespace LifeTimeBot.Endpoints.Ai.GetDailyAnalysis;

sealed class GetDailyAnalysisRequest 
{
    public long BotId { get; set; }
    public long ChatId { get; set; }
    public DateTime Date { get; set; }
}

sealed class GetDailyAnalysisResponse 
{
    public bool IsSuccess { get; set; }
    public string? AiAnswer { get; set; }
    public string? Error { get; set; }
}

sealed class GetDailyAnalysisEndpoint : Endpoint<GetDailyAnalysisRequest, GetDailyAnalysisResponse>
{
    private AppDbContext _db;
    private readonly ActivityService _activityService;
    private readonly LlmService _llmService;
    private readonly BotResources R;

    public GetDailyAnalysisEndpoint(AppDbContext db, ActivityService activityService, LlmService llmService, 
        IOptions<BotResources> options)
    {
        _db = db;
        _activityService = activityService;
        _llmService = llmService;
        R = options.Value;
    }

    public override void Configure()
    {
        Get("/daily-analysis");
        AllowAnonymous();
        Group<AiGroup>();
        Summary(s =>
        {
            s.Summary = "Получить анализ за текущий день";
            s.Description = $"Получить анализ за текущий день";
        });
    }

    public override async Task HandleAsync(GetDailyAnalysisRequest r, CancellationToken c)
    {
        var start = r.Date.Date;
        var end = r.Date.AddDays(1).Date;

        GetActivitiesDto dto = new()
        {
            Start = start,
            End = end,
            Confirmed = true,
            CropDates = false,
            StrictRestrictions = false,
            BotIds = new List<long>() { r.BotId },
            ChatIds = new List<long>() { r.ChatId },
            Order = $"{nameof(ActivityEntity.StartTime)}"
        };
        
        var res = await _activityService.GetActivities(dto);
        
        if(res.Data is null || res.Data.Count < R.MinActivitiesCountForAiDailyRecommendation)
        {
            await SendAsync(new GetDailyAnalysisResponse()
            {
                IsSuccess = false, Error = R.NotEnoughActivityForAiDailyRecommendation
            });
            return;
        }

        string aiAnswer = await _llmService.GetRecommendationsForActivities(res.Data);

        GetDailyAnalysisResponse response = new()
        {
            IsSuccess = true,
            AiAnswer = aiAnswer
        };
        
        await SendAsync(response);
    }
}