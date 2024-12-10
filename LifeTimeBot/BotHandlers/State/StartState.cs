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

[BotHandler(stateName:Name, version: 2.0)]
public class StartState: BaseLifeTimeBotHandler
{
    public const string Name = "StartState";
    
    public StartState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override async Task HandleBotRequest(Update update)
    {
        var utc = AppConstants.UTCTimezones;
        InlineKeyboardBuilder kb = new InlineKeyboardBuilder();
        kb.NewRow().Add(utc[12].Title, R.BtnChooseUtcCallback(utc[12].Difference));
        for (int i = 0; i < 12; i++)
        {
            kb.NewRow()
                .Add(utc[0+i].Title, R.BtnChooseUtcCallback(utc[0+i].Difference))
                .Add(utc[13+i].Title, R.BtnChooseUtcCallback(utc[13+i].Difference));
        }

        await Answer(R.ChooseUtc, replyMarkup: kb.Build());
    }
}