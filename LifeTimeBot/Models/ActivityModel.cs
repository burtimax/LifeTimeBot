using System.Text.Json.Serialization;
using LifeTimeBot.Db.AppDb.Entities;
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
    
    [JsonPropertyName("type")]
    public ActivityType Type { get; set; }
    
    [JsonPropertyName("balance")]
    public List<BalanceType> BalanceTypes { get; set; }


    public ActivityEntity ToEntity(long botId, long chatId, int utc, string? fileId = null, int? messageId = null, string? messageText = null)
    {
        // Погрешность, если пользователь указал время окончания чуть больше текущего времени.
        int hoursError = 2;
        
        DateTime startTime = DateTime.Parse(StartTime);
        DateTime endTime = DateTime.Parse(EndTime);
        
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
        };
    }
}