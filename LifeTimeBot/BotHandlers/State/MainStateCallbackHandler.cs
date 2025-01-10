using System.Diagnostics.Tracing;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using MetalBoardBot.BotHandlers.Command;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Utils.Keyboard;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State;

[BotHandler(stateName:MainState.Name, updateTypes:new []{UpdateType.CallbackQuery}, version: 2)]
public class MainStateCallbackHandler: BaseLifeTimeBotHandler
{
    private readonly ActivityService _activityService;
    private readonly AppDbContext _db;
    
    public MainStateCallbackHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _activityService = serviceProvider.GetRequiredService<ActivityService>();
        _db = serviceProvider.GetRequiredService<AppDbContext>();
        Expected(UpdateType.CallbackQuery);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        string data = callbackQuery.Data!.ToString();
        int messageId = callbackQuery.Message?.MessageId ?? throw new Exception("MessageId is null");
        await AnswerCallback();
        
        if (data.StartsWith(R.BtnConfirmActivityKey))
        {
            long activityId = long.Parse(data.Replace(R.BtnConfirmActivityKey, ""));
            await _activityService.ConfirmActivity(activityId);
            await ChangeActivityMessage(messageId, true);
            //await DeleteMessage(callbackQuery.Message?.MessageId);
            //await SendTodayList();
            return;
        }

        if (data.StartsWith(R.BtnCancelActivityKey))
        {
            long activityId = long.Parse(data.Replace(R.BtnCancelActivityKey, ""));
            await _activityService.CancelActivity(activityId);
            await ChangeActivityMessage(messageId, true);
            //await DeleteMessage(callbackQuery.Message?.MessageId);
            //await SendTodayList();
            return;
        }
        
        if (data.StartsWith(R.BtnOpenBalanceTypesKey))
        {
            long activityId = long.Parse(data.Replace(R.BtnOpenBalanceTypesKey, ""));
            ActivityEntity? entity = await _activityService.GetActivityById(activityId);
            if(entity is null) return;
            await (this.GetHandlerInstance<MainState>() as MainState).SendActivityEntity(entity, true,
                callbackQuery.Message.MessageId);
            return;
        }
        
        if (data.StartsWith(R.AddBalanceTypeToActivityKey))
        {
            long activityId = long.Parse(data.Split(':')[1]);
            BalanceType bType = (BalanceType) int.Parse(data.Split(':')[2]);
            ActivityEntity? entity = await _activityService.GetActivityById(activityId);
            if (entity.BalanceTypes.Contains(bType)) return;
            entity.BalanceTypes.Add(bType);
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            await (this.GetHandlerInstance<MainState>() as MainState).SendActivityEntity(entity, true,
                callbackQuery.Message.MessageId);
            return;
        }
        
        if (data.StartsWith(R.RemoveBalanceTypeFromActivityKey))
        {
            long activityId = long.Parse(data.Split(':')[1]);
            BalanceType bType = (BalanceType) int.Parse(data.Split(':')[2]);
            ActivityEntity? entity = await _activityService.GetActivityById(activityId);
            if (entity.BalanceTypes.Contains(bType) == false) return;
            entity.BalanceTypes.RemoveAll(t => t == bType);
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            await (this.GetHandlerInstance<MainState>() as MainState).SendActivityEntity(entity, true,
                callbackQuery.Message.MessageId);
            return;
        }
        
        if (data.StartsWith(R.BtnShowActivityListFor24HoursCallbackKey))
        {
            await SendTodayList();
            return;
        }
    }

    private async Task SendTodayList()
    {
        await this.HandleBotRequest<TodayCommand>();
    }

    private async Task ChangeActivityMessage(int messageId, bool activitySaved)
    {
        string text = activitySaved ? R.ActivityWasSaved : R.ActivityWasDeleted;

        InlineKeyboardBuilder kb = new();
        kb.NewRow().Add(R.BtnShowActivityListFor24Hours, R.BtnShowActivityListFor24HoursCallbackKey);

        try
        {
            await BotClient.EditMessageTextAsync(Chat.ChatId, messageId, text: text, replyMarkup: kb.Build());
        }
        catch (Exception e)
        {
            await BotClient.SendMessageAsync(Chat.ChatId, text: text, replyMarkup: kb.Build());
        }
        
    }
}