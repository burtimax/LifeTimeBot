using LifeTimeBot.App.Constants;
using LifeTimeBot.App.Options;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Models;
using LifeTimeBot.Services;
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
public partial class MainState: BaseLifeTimeBotHandler
{
    public const string Name = "MainState";
 
    private readonly LlmService llm;
    private readonly AsrService asr;
    private readonly ActivityService _activityService;
    
    public MainState(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        this.llm = serviceProvider.GetRequiredService<LlmService>();
        this.asr = serviceProvider.GetRequiredService<AsrService>();
        this._activityService = serviceProvider.GetRequiredService<ActivityService>();
        NotExpectedMessage = R.DontUnderstandYou;
        Expected(UpdateType.Message);
        ExpectedMessage(MessageType.Text, MessageType.Voice);
    }

    public override async Task SendIntroduction()
    {
        await Answer(R.MainStateIntroduction);
    }

    public override async Task HandleMessage(Message message)
    {
        if (message.Type() == MessageType.Voice)
        {
            await HandleVoiceMessage(message);
        }
        
        if (message.Type() == MessageType.Text)
        {
            await HandleTextMessage(message);
        }
    }

    private async Task HandleTextMessage(Message message)
    {
        Message mesInProgress = await Answer(R.InProgress);
        bool mesExistedInProgress = true;

        async Task DeleteInProgress()
        {
            if (mesExistedInProgress)
            {
                await BotClient.DeleteMessageAsync(Chat.ChatId, mesInProgress.MessageId);
                mesExistedInProgress = false;
            }
        } 
        
        string text = message.Text;
        var resActivity = await llm.GetActivityDataFromText(text);
        
        // ИИ не распознала активность
        if (resActivity.Activity is null)
        {
            // написать пользователю как стоит оформить сообщение.
            await DeleteInProgress();
            await Answer(string.Format(R.NotFoundActivityInText, text));
            return;
        }
        
        ActivityModel activity = resActivity.Activity;
        if (activity.HasErrors(out _))
        {
            await Answer(string.Format(R.NotFoundActivityInText, text));
            return;
        }
        
        ActivityEntity entity = activity.ToEntity(BotId, Chat.ChatId, GetUserUtc()!.Value, messageId: Update.Message.MessageId, messageText: text);
        await _activityService.SaveActivity(entity);

        await DeleteInProgress();
        await SendActivityEntity(entity);
        return;
    }
    
    private async Task HandleVoiceMessage(Message message)
    {
        var voice = message.Voice;
        if (voice.Duration > 20)
        {
            await Answer(R.TooLongVoice);
            return;
        }

        Message mesInProgress = await Answer(R.InProgress);
        bool mesExistedInProgress = true;

        async Task DeleteInProgress()
        {
            if (mesExistedInProgress)
            {
                await BotClient.DeleteMessageAsync(Chat.ChatId, mesInProgress.MessageId);
                mesExistedInProgress = false;
            }

            await DeleteRecognized();
        } 
        
        bool mesExistedRecognized = true;
        Message mesRecognized = null;
        async Task DeleteRecognized()
        {
            if (mesExistedRecognized && mesRecognized != null)
            {
                await BotClient.DeleteMessageAsync(Chat.ChatId, mesRecognized.MessageId);
                mesExistedRecognized = false;
            }
        } 
        
        DownloadedTelegramFile telegramFile = await BotClient.DownloadFileAsync(voice.FileId);
        var recognisedText =await asr.WhisperVoiceToText(telegramFile.FileData);
        if (string.IsNullOrEmpty(recognisedText) == false)
        {
            mesRecognized = await Answer(recognisedText);
        }
        else
        {
            await DeleteInProgress();
            await Answer(R.CannotRecognizeTextInVoice);
        }

        //string text = "погладить кошечку 1200 1345";
        var resActivity = await llm.GetActivityDataFromText(recognisedText);
        
        // ИИ не распознала активность
        if (resActivity.Activity is null)
        {
            // написать пользователю как стоит оформить сообщение и вывести, что нейронка услышала.
            await DeleteInProgress();
            await Answer(string.Format(R.NotFoundActivityInVoice, recognisedText));
            return;
        }
        
        // Есть активность.
        ActivityModel activity = resActivity.Activity;
        if (activity.HasErrors(out _))
        {
            await Answer(string.Format(R.NotFoundActivityInText, recognisedText));
            return;
        }
        
        int utc = User.AdditionalProperties.Get<int>(AppConstants.UserUtcPropKey);
        ActivityEntity entity = activity.ToEntity(BotId, Chat.ChatId, utc, voice.FileId, Update.Message.MessageId, recognisedText);
        await _activityService.SaveActivity(entity);
        
        await DeleteInProgress();
        await SendActivityEntity(entity);
        return;
    }

    public async Task SendActivityEntity(ActivityEntity entity, bool openBalanceTypes = false, int? messageId = null)
    {
        bool HasBalance(BalanceType bType) => entity.BalanceTypes.Contains(bType);

        string text = string.Format(R.ActivityTemplate, entity.StartTime.Value.ToString(AppConstants.TimeFormat),
            entity.EndTime.Value.ToString(AppConstants.TimeFormat), entity.Description);
        
        InlineKeyboardButton GetBtn(BalanceType bType)
        {
            string name = HasBalance(bType)
                ? string.Format(R.BtnBalanceContainsNameFormat, R.AppResources.GetBalanceTypeName(bType))
                : string.Format(R.BtnBalanceNotContainsNameFormat, R.AppResources.GetBalanceTypeName(bType));
            string callback = HasBalance(bType)
                ? R.RemoveBalanceTypeFromActivityCallback(entity.Id, bType)
                : R.AddBalanceTypeToActivityCallback(entity.Id, bType);
            return new InlineKeyboardButton(name)
            {
                CallbackData = callback
            };
        }
        
        InlineKeyboardBuilder kb = new InlineKeyboardBuilder();

        if (openBalanceTypes)
        {
            kb.NewRow()
                .Add(GetBtn(BalanceType.СareerWorkBusiness))
                .Add(GetBtn(BalanceType.LoveFamilyChildren))
                .NewRow()
                .Add(GetBtn(BalanceType.HealthyAndSport))
                .Add(GetBtn(BalanceType.FriendsAndCommunity))
                .NewRow()
                .Add(GetBtn(BalanceType.HobbyAndBrightnessOfLife))
                .Add(GetBtn(BalanceType.PersonalDevelopmentAndEducation))
                .NewRow()
                .Add(GetBtn(BalanceType.Finance))
                .Add(GetBtn(BalanceType.Spirituality));
        }
        else
        {
            kb.NewRow()
                .Add(R.BtnOpenBalanceTypes, R.BtnOpenBalanceTypesKeyCallback(entity.Id));
        }
            
        kb.NewRow()
        .Add(R.BtnConfirmActivity, R.BtnConfirmActivityCallback(entity.Id))
        .Add(R.BtnCancelActivity, R.BtnCancelActivityCallback(entity.Id));

        if (messageId.HasValue)
        {
            try
            {
                await BotClient.EditMessageReplyMarkupAsync(Chat.ChatId, messageId.Value, replyMarkup: kb.Build());
                return;
            }catch (Exception e) { }
        }
        
        await Answer(text, replyMarkup: kb.Build());
    }
}