using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<ActivityEntity>> GetTodayActivities(long chatId, int utcOffset)
    {
        DateTime now = DateTime.UtcNow.AddHours(utcOffset);
        
        
        
        var activities = await _db.Activity.Where(a => a.Confirmed
                                && a.TelegramChatId == chatId
                                && 
                                (
                                    (a.StartTime >= now.Date.AddDays(-1) && a.StartTime < now) 
                                ||  
                                    (a.EndTime >= now.Date.AddDays(-1) && a.EndTime < now.Date)
                                )
                                )
                                .OrderBy(a => a.StartTime)
            .AsNoTracking()
            .ToListAsync();

        // if (activities.Any())
        // {
        //     activities.ForEach(a =>
        //     {
        //         if(a.EndTime > now.Date.AddDays(1)) a.EndTime = now.Date.AddDays(1).AddMinutes(-1);
        //         if(a.StartTime < now.Date) a.StartTime = now.Date;
        //     });
        // }
        
        return activities;
    }
}