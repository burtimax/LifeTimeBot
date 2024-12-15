using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LifeTimeBot.App.Constants;
using LifeTimeBot.BotHandlers.State;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.LLM;
using MultipleBotFramework;
using MultipleBotFramework.Attributes;
using MultipleBotFramework.Constants;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Utils.Keyboard;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;

namespace MetalBoardBot.BotHandlers.Command;

[BotHandler(command: "/help", requiredUserClaims: new []{BotConstants.BaseBotClaims.IAmBruceAlmighty})]
public class HelpCommand : BaseLifeTimeBotHandler
{
    
    public HelpCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override async Task HandleBotRequest(Update update)
    {
        InlineKeyboardBuilder kb = new();
        kb.NewRow().Add(new InlineKeyboardButton(R.BtnHelp)
        {
            Url = R.HelpUrl
        });

        await Answer(R.HelpDescription, replyMarkup: kb.Build());
    }
}