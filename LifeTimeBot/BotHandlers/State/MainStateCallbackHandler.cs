using System.Diagnostics.Tracing;
using LifeTimeBot.Services;
using MetalBoardBot.BotHandlers.Command;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.AvailableTypes;

namespace LifeTimeBot.BotHandlers.State;

[BotHandler(stateName:MainState.Name, updateTypes:new []{UpdateType.CallbackQuery}, version: 2)]
public class MainStateCallbackHandler: BaseLifeTimeBotHandler
{
    private readonly ActivityService _activityService;
    
    public MainStateCallbackHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _activityService = serviceProvider.GetRequiredService<ActivityService>();
        Expected(UpdateType.CallbackQuery);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        string data = callbackQuery.Data!.ToString();
        await AnswerCallback();
        
        if (data.StartsWith(R.BtnConfirmActivityKey))
        {
            long activityId = long.Parse(data.Replace(R.BtnConfirmActivityKey, ""));
            await _activityService.ConfirmActivity(activityId);
            await DeleteMessage(callbackQuery.Message?.MessageId);
            await SendTodayList();
            return;
        }

        if (data.StartsWith(R.BtnCancelActivityKey))
        {
            long activityId = long.Parse(data.Replace(R.BtnCancelActivityKey, ""));
            await _activityService.CancelActivity(activityId);
            await DeleteMessage(callbackQuery.Message?.MessageId);
            await SendTodayList();
            return;
        }
    }

    private async Task SendTodayList()
    {
        await this.HandleBotRequest<TodayCommand>();
    }
}