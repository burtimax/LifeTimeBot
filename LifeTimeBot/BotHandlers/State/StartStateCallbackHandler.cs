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
public class StartStateCallbackHandler: BaseLifeTimeBotState
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
            int diff = int.Parse(data.Substring(R.BtnChooseUtcKey.Length));
            User.AdditionalProperties.Set(AppConstants.UserUtcPropKey, diff);
            BotDbContext.Update(User);
            await BotDbContext.SaveChangesAsync();
            await Answer(string.Format(R.UserChosenUtc, AppConstants.UTCTimezones.Where(u => u.Difference == diff).First().Title));
            await ChangeState(MainState.Name);
            await this.GetHandlerInstance<MainState>().SendIntroduction();
        }
    }
}