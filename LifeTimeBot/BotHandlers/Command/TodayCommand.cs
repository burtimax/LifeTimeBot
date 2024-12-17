using System.Text;
using LifeTimeBot.App.Constants;
using LifeTimeBot.BotHandlers.State;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using MultipleBotFramework.Attributes;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;

namespace MetalBoardBot.BotHandlers.Command;

[BotHandler(command: "/today", version: 2.0f)]
public class TodayCommand : BaseLifeTimeBotHandler
{
    private const string lastTodayMes = "lastToday";
    private readonly ActivityService _activityService;
    
    public TodayCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _activityService = serviceProvider.GetRequiredService<ActivityService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        if (GetUserUtc() == null)
        {
            await Answer(R.NoActivitiesToday);
            return;
        }
        
        List<ActivityEntity> activities = await _activityService.Get24HoursActivities(BotId, Chat.ChatId, GetUserUtc()!.Value);
        
        if (activities is null || activities.Any() == false)
        {
            await Answer(R.NoActivitiesToday);
            return;
        }
        
        StringBuilder sb = new StringBuilder();
        foreach (var ac in activities)
        {
            sb.AppendLine(string.Format(R.ActivityListItemTemplate, ac.StartTime.Value.ToString(AppConstants.TimeFormat), ac.EndTime.Value.ToString(AppConstants.TimeFormat), ac.Description, ac.Emoji));
        }

        if (Chat.Data.Contains(lastTodayMes))
        {
            int lastToday = Chat.Data.Get<int>(lastTodayMes);
            await DeleteMessage(lastToday);
        }

        Message mes = await Answer(sb.ToString());
        Chat.Data.Set(lastTodayMes, mes.MessageId);
        await BotDbContext.SaveChangesAsync();
    }
    
    
}