using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Models;
using LifeTimeBot.Services.Dto;
using Microsoft.EntityFrameworkCore;
using MultipleBotFrameworkEndpoints.Extensions;
using MultipleBotFrameworkEndpoints.Models;

namespace LifeTimeBot.Services;

public class ActivityService
{
    private readonly AppDbContext _db;

    public ActivityService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<ActivityEntity?> SaveActivity(ActivityEntity activity)
    {
        _db.Activity.Add(activity);
        await _db.SaveChangesAsync();
        return activity;
    }

    public async Task<ActivityEntity?> ConfirmActivity(long id)
    {
        ActivityEntity? activity = await GetActivityById(id);
        if (activity == null) return null;
        
        activity.Confirmed = true;
        _db.Activity.Update(activity);
        await _db.SaveChangesAsync();
        return activity;
    }
    
    public async Task<ActivityEntity?> CancelActivity(long id)
    {
        ActivityEntity? activity = await GetActivityById(id);
        if (activity == null) return null;
        
        activity.Confirmed = false;
        _db.Activity.Update(activity);
        await _db.SaveChangesAsync();
        return activity;
    }

    public async Task<ActivityEntity?> GetActivityById(long id)
    {
        return _db.Activity.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<ActivityEntity>> Get24HoursActivities(long botId, long chatId, int utcOffset)
    {
        DateTime now = DateTime.UtcNow.AddHours(utcOffset);
        
        DateTime start = now.AddDays(-1);
        DateTime end = now;

        GetActivitiesDto dto = new()
        {
            Start = start,
            End = end,
            BotIds = new List<long>() { botId },
            ChatIds = new List<long>() { chatId },
            Confirmed = true,
            Order = nameof(ActivityEntity.StartTime)
        };

        var activities = await GetActivities(dto, true);
        
        return activities.Data;
    }
    
    public async Task<PagedList<ActivityEntity>> GetActivities(GetActivitiesDto dto, bool asNoTracking = false)
    {
        IQueryable<ActivityEntity> activities = _db.Activity
                .WhereIf(dto.Confirmed != null, a => a.Confirmed == dto.Confirmed)
                .WhereIf(dto.Ids is not null && dto.Ids.Any(), a => dto.Ids.Contains(a.Id))
                .WhereIf(dto.BotIds is not null && dto.BotIds.Any(), a => dto.BotIds.Contains(a.BotId))
                .WhereIf(dto.ChatIds is not null && dto.ChatIds.Any(), a => dto.ChatIds.Contains(a.TelegramChatId))
                .WhereIf(dto.Start is not null, a => a.StartTime >= dto.Start || (!dto.StrictRestrictions && a.EndTime > dto.Start))
                .WhereIf(dto.End is not null, a => a.EndTime <= dto.End || (!dto.StrictRestrictions && a.StartTime < dto.End))
                .Order(dto.Order);

        if (asNoTracking)
            activities = activities.AsNoTracking();

        var res = await PagedList<ActivityEntity>.ToPagedListAsync(activities, Pagination.All);
        
        if ((res.Data?.Count ?? 0) > 0 && dto.CropDates)
        {
            for (int i = 0; i < res.Data.Count; i++)
            {
                var a = res.Data[i];
                
                var start = dto.Start ?? DateTimeOffset.MinValue;
                var end = dto.End ?? DateTimeOffset.MaxValue;
                if(a.StartTime is not null)
                    a.StartTime = a.StartTime.Value.AddHours(-1 * a.StartTime.Value.Offset.TotalHours);
                if(a.EndTime is not null)
                    a.EndTime = a.EndTime.Value.AddHours(-1 * a.EndTime.Value.Offset.TotalHours);
                
                if(a.StartTime is not null && a.StartTime < start) a.StartTime = start;
                if(a.EndTime is not null && a.EndTime > end) a.EndTime = end;
            }
        }

        return res;
    }
}