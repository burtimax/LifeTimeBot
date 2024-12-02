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
public partial class MainState: BaseLifeTimeBotState
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
            var recognisedText =await asr.VoiceToText(telegramFile.FileData);
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

            int utc = User.AdditionalProperties.Get<int>(AppConstants.UserUtcPropKey);
            ActivityEntity entity = activity.ToEntity(Chat.ChatId, utc, voice.FileId, Update.Message.MessageId, recognisedText);
            await _activityService.SaveActivity(entity);

            InlineKeyboardBuilder kb = new InlineKeyboardBuilder()
                .NewRow()
                .Add(R.BtnConfirmActivity, R.BtnConfirmActivityCallback(entity.Id))
                .Add(R.BtnCancelActivity, R.BtnCancelActivityCallback(entity.Id));
            
            await DeleteInProgress();
            await Answer(string.Format(R.ActivityTemplate, entity.StartTime.Value.ToString(AppConstants.DateTimeFormat), entity.EndTime.Value.ToString(AppConstants.DateTimeFormat), activity.Action), replyMarkup: kb.Build());
            return;
        }
        else
        {
            await Answer(R.DontUnderstandYou);
        }
    }

    
}