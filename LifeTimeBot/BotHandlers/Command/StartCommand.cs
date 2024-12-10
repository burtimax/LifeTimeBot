using LifeTimeBot.BotHandlers.State;
using MultipleBotFramework.Attributes;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.GettingUpdates;

namespace MetalBoardBot.BotHandlers.Command;

[BotHandler(command: "/start", version: 2.0f)]
public class StartCommand : BaseLifeTimeBotHandler
{
    public StartCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override async Task HandleBotRequest(Update update)
    {
        await ChangeState(StartState.Name);
        await this.HandleBotRequest<StartState>();
    }
}