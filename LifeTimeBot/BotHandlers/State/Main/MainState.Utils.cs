using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services.LLM.Dto;
using MultipleBotFramework.Extensions.ITelegramApiClient;
using MultipleBotFramework.Models;
using MultipleBotFramework.Utils.Keyboard;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State.Main;

public partial class MainState
{
    
    async Task DeleteInProgress()
    {
        if (_mesExistedInProgress)
        {
            await BotClient.DeleteMessageAsync(Chat.ChatId, _mesInProgress.MessageId);
            _mesExistedInProgress = false;
        }
    } 
    
    private async Task<LlmActivityDataResult> GetActivitiesFromText(string text)
    {
        try
        {
            var ac24 = await _activityService.Get24HoursActivities(BotId, Chat.ChatId, GetUserUtc()!.Value);
            string lastUserActivityTime = ac24?.LastOrDefault()?.EndTime != null ? $"{ac24.Last().EndTime!.Value.Hour}:{ac24.Last().EndTime!.Value.Minute}" : "00:00";
            
            return await llm.GetActivityDataFromText(text, lastUserActivityTime);
        }
        catch (Exception e)
        {
            await Answer(R.NotFoundActivityInText);
            throw;
        }
    }
    
    private async Task<string?> RecognizeTextFromVoice(byte[] voiceData)
    {
        try
        {
            return await asr.WhisperVoiceToText(voiceData);
        }
        catch (Exception e)
        {
            await Answer(R.BotExceptions.CannotRecognizeTextFromAudio);
            throw;
        }
    }
    
    private async Task<string?> GetTextFromVoice(Voice voice)
    {
        DownloadedTelegramFile telegramFile = await BotClient.DownloadFileAsync(voice.FileId);
        var recognisedText = await RecognizeTextFromVoice(telegramFile.FileData);
        if (string.IsNullOrEmpty(recognisedText) == false ||
            string.Equals(recognisedText.Trim(' '), "you", StringComparison.OrdinalIgnoreCase) == false)
        {
            await Answer(recognisedText);
            return recognisedText;
        }
        else
        {
            await DeleteInProgress();
            await Answer(R.CannotRecognizeTextInVoice);
            return null;
        }
    }
    
    private async Task ProcessActivities(string activitiesText, string? voiceFileId = null)
    {
        var resActivity = await GetActivitiesFromText(activitiesText);
        
        // ИИ не распознала активность
        if (resActivity.Activities is null || resActivity.Activities.Any() == false)
        {
            // написать пользователю как стоит оформить сообщение и вывести, что нейронка услышала.
            await DeleteInProgress();
            await Answer(string.Format(R.NotFoundActivityInVoice, activitiesText));
            return;
        }
        
        // Есть активности.
        foreach (var activity in resActivity.Activities)
        {
            if (activity.HasErrors(out _))
            {
                await Answer(string.Format(R.NotFoundActivityInText, activitiesText));
                return;
            }
        
            ActivityEntity entity = activity.ToEntity(BotId, Chat.ChatId, GetUserUtc()!.Value, voiceFileId, Update.Message.MessageId, activitiesText);
            await _activityService.SaveActivity(entity);
        
            await DeleteInProgress();
            await SendActivityEntity(entity);
        }
    }
    
    public async Task SendActivityEntity(ActivityEntity entity, bool openBalanceTypes = false, int? messageId = null)
    {
        bool HasBalance(BalanceType bType) => entity.BalanceTypes.Contains(bType);

        string text = string.Format(R.ActivityTemplate, entity.StartTime.Value.ToString(AppConstants.TimeFormat),
            entity.EndTime.Value.ToString(AppConstants.TimeFormat), entity.Description, entity.Emoji);
        
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