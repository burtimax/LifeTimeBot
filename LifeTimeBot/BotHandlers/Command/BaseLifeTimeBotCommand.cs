// using LifeTimeBot.App.Constants;
// using LifeTimeBot.Resources;
// using Microsoft.Extensions.Options;
// using MultipleBotFramework;
// using MultipleBotFramework.Base;
// using MultipleBotFramework.Constants;
// using MultipleBotFramework.Enums;
// using MultipleBotFramework.Extensions;
// using Telegram.BotAPI.AvailableMethods;
// using Telegram.BotAPI.AvailableTypes;
// using Telegram.BotAPI.GettingUpdates;
// using Telegram.BotAPI.UpdatingMessages;
//
// namespace MetalBoardBot.BotHandlers.Command;
//
// public class BaseLifeTimeBotCommand : BaseBotHandler
// {
//     protected readonly BotResources R;
//
//     public BaseLifeTimeBotCommand(IServiceProvider serviceProvider) : base(serviceProvider)
//     {
//         R = serviceProvider.GetRequiredService<IOptions<BotResources>>().Value;
//     }
//
//     protected Task<Message> Answer(string text, string parseMode = ParseMode.Html, ReplyMarkup replyMarkup = default)
//     {
//         return BotClient.SendMessageAsync(Chat.ChatId, text:text, parseMode:parseMode, replyMarkup: replyMarkup);
//     }
//     
//     protected async Task AnswerCallback()
//     {
//         if (this.Update.Type() == MultipleBotFramework.Enums.UpdateType.CallbackQuery)
//         {
//             await BotClient.AnswerCallbackQueryAsync(this.Update.CallbackQuery.Id);
//         }
//     }
//     
//     protected async Task DeleteMessage(int? messageId = null)
//     {
//         int? mesId = messageId == null ? Update?.Message?.MessageId : messageId;
//         if(mesId is null) return;
//         try
//         {
//             await BotClient.DeleteMessageAsync(Chat.ChatId, mesId.Value);
//         }
//         catch (Exception e) { }
//     }
//     
//     public async Task ChangeState(string stateName, ChatStateSetterType setterType = ChatStateSetterType.ChangeCurrent)
//     {
//         Chat.States.Set(stateName, setterType);
//         await BotDbContext.SaveChangesAsync();
//     }
//
//     public override Task HandleBotRequest(Update update)
//     {
//         throw new NotImplementedException();
//     }
// }