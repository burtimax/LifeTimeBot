using LifeTimeBot.App.Constants;
using LifeTimeBot.Models;
using LifeTimeBot.Services.ASR;
using LifeTimeBot.Services.LLM;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Extensions.ITelegramApiClient;
using MultipleBotFramework.Models;
using MultipleBotFramework.Utils.Keyboard;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State;

[BotHandler(stateName:StartState.Name, updateTypes:new [] { UpdateType.CallbackQuery }, version: 2.0)]
public class StartStateCallbackHandler: BaseLifeTimeBotHandler
{
    public StartStateCallbackHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Expected(UpdateType.CallbackQuery);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        string data = callbackQuery.Data;
        await AnswerCallback();
        if (data.StartsWith(R.BtnChooseUtcKey))
        {
            int offset = int.Parse(data.Substring(R.BtnChooseUtcKey.Length));
            await SetUserUtc(offset);
            await Answer(string.Format(R.UserChosenUtc, AppConstants.UTCTimezones.Where(u => u.Difference == offset).First().Title));
            await ChangeState(MainState.Name);
            await this.GetHandlerInstance<MainState>().SendIntroduction();
        }
    }
}