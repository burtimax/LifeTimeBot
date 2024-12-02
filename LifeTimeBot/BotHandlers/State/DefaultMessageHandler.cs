using LifeTimeBot.Models;
using LifeTimeBot.Services.ASR;
using LifeTimeBot.Services.LLM;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Extensions.ITelegramApiClient;
using MultipleBotFramework.Models;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State;

[BotHandler(version: 2.0, updateTypes:new []{UpdateType.Message})]
public class DefaultMessageHandler: BaseLifeTimeBotState
{
 
    
    public DefaultMessageHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override async Task HandleBotRequest(Update update)
    {
        await Answer(R.DontUnderstandYou);
    }
}