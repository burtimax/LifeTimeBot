using System.Text.Json.Serialization;
using LifeTimeBot.Db.AppDb.Entities;

namespace LifeTimeBot.Models;

public class ActivityModel
{
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public string EndTime { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }


    public ActivityEntity ToEntity(long chatId, int utc, string? fileId, int? messageId, string? messageText)
    {
        // Погрешность, если пользователь указал время окончания чуть больше текущего времени.
        int hoursError = 2;
        
        DateTime startTime = DateTime.Parse(StartTime);
        DateTime endTime = DateTime.Parse(EndTime);
        
        DateTime now = DateTime.UtcNow.AddHours(utc);
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
        
        return new ActivityEntity()
        {
            Confirmed = false,
            StartTime = startDate,
            EndTime = endDate,
            Description = Action,
            TelegramChatId = chatId,
            AudioFileId = fileId,
            MessageId = messageId,
            MessageText = messageText,
        };
    }
}