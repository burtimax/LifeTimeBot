using System.Diagnostics;
using System.Text;
using LifeTimeBot.App.Constants;
using LifeTimeBot.BotHandlers.State;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.LLM;
using MultipleBotFramework.Attributes;
using MultipleBotFramework.Constants;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;

namespace MetalBoardBot.BotHandlers.Command;

[BotHandler(command: "/advice", version: 2.0f)]
public class AdviceCommand : BaseLifeTimeBotHandler
{
    private readonly ActivityService _activityService;
    private readonly LlmService _llmService;
    
    public AdviceCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _activityService = serviceProvider.GetRequiredService<ActivityService>();
        _llmService = serviceProvider.GetRequiredService<LlmService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        if (GetUserUtc() == null)
        {
            await Answer(R.NoActivitiesToday);
            return;
        }
        
        List<ActivityEntity> activities = await _activityService.Get24HoursActivities(BotId, Chat.ChatId, GetUserUtc()!.Value);

        // Недостаточно активностей для получения рекомендаций.
        if (activities is null || activities.Count < R.MinActivitiesCountForAiDailyRecommendation)
        {
            await Answer(R.NotEnoughActivityForAiDailyRecommendation);
            return;
        }

        var inProgress = await Answer(R.InProgress);
        
        string dailyRoutine = ActivitiesToString(activities);
        string? recommendations = await _llmService.GetRecommendationsForRoutine(dailyRoutine);

        await DeleteMessage(inProgress.MessageId);
        await this.HandleBotRequest<TodayCommand>();
        await Answer(recommendations ?? R.NoRecommendationsForDaily, parseMode: ParseMode.Markdown);
    }

    private string ActivitiesToString(List<ActivityEntity> activities)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var ac in activities)
        {
            sb.AppendLine(string.Format(R.ActivityListItemTemplate, ac.StartTime.Value.ToString(AppConstants.TimeFormat), ac.EndTime.Value.ToString(AppConstants.TimeFormat), ac.Description));
        }

        return sb.ToString();
    }
}