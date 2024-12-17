using System.Text.Json.Serialization;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Endpoints.Daily.GetDailyCalendarPositions;
using LifeTimeBot.Extensions;

namespace LifeTimeBot.Models;

public class ActivityModel
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }
    [JsonPropertyName("emoji")]
    public string Emoji { get; set; }
    
    [JsonPropertyName("type")]
    public ActivityType Type { get; set; }
    
    [JsonPropertyName("balance")]
    public List<BalanceType> BalanceTypes { get; set; }

    public bool HasErrors(out List<string> errors)
    {
        bool hasErrors = false;
        errors = new();

        if (HasTimeError(StartTime))
        {
            hasErrors = true;
            errors.Add($"Время начала неправильное!");
        }
        
        if (HasTimeError(EndTime))
        {
            hasErrors = true;
            errors.Add($"Время окончания неправильное!");
        }

        if (string.IsNullOrEmpty(Action.Trim(' ')))
        {
            hasErrors = true;
            errors.Add($"Укажите занятие!");
        }
        
        return hasErrors;
    }

    private bool HasTimeError(string time)
    {
        int h = int.Parse(time.Split(":")[0]);
        int m = int.Parse(time.Split(":")[1]);

        if (h < 0 || h > 24) return true;
        if (m < 0 || m > 59) return true;

        return false;
    }
    
    private string GetValidTime(string time)
    {
        int h = int.Parse(time.Split(":")[0]);
        int m = int.Parse(time.Split(":")[1]);
        if(h == 24) h = 0;
        
        if (HasTimeError(time)) return "00:00";
        return $"{h:00}:{m:00}";
    }
    
    public ActivityEntity ToEntity(long botId, long chatId, int utc, string? fileId = null, int? messageId = null, string? messageText = null)
    {
        // Погрешность, если пользователь указал время окончания чуть больше текущего времени.
        int hoursError = 2;
        
        DateTime startTime = DateTime.Parse(GetValidTime(StartTime));
        DateTime endTime = DateTime.Parse(GetValidTime(EndTime));
        
        // Нельзя менять время местами, потому что запись активности может происходить на перемене дат
        // Например: С 23:00 до 01:00: Работа над приложением
        // if (endTime < startTime) (startTime, endTime) = (endTime, startTime);
        
        DateTime now = DateTime.UtcNow.AddHours(utc);
        startTime = new DateTime(now.Year, now.Month, now.Day, startTime.Hour, startTime.Minute, startTime.Second);
        endTime = new DateTime(now.Year, now.Month, now.Day, endTime.Hour, endTime.Minute, endTime.Second);
        
        DateTime endDate = now.Date;
        DateTime startDate = now.Date;
        if (endTime > now.AddHours(hoursError))
        {
            endDate = endDate.AddDays(-1);
            startDate = endDate;
        }
        
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
        
        if (startDate > endDate)
        {
            startDate = endDate.AddDays(-1);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
        }

        // Приводим к виду предложения.
        if (string.IsNullOrEmpty(this.Action.Trim(' ')) == false)
        {
            this.Action = this.Action.Trim(' ').FirstCharToUpper();
        }
        
        return new ActivityEntity()
        {
            BotId = botId,
            Confirmed = false,
            StartTime = startDate,
            EndTime = endDate,
            Description = this.Action,
            TelegramChatId = chatId,
            AudioFileId = fileId,
            MessageId = messageId,
            MessageText = messageText,
            Type = this.Type,
            BalanceTypes = this.BalanceTypes,
            Emoji = Emoji
        };
    }
}