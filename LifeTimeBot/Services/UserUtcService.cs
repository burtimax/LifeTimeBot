using LifeTimeBot.App.Constants;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Db;
using MultipleBotFramework.Db.Entity;
using Telegram.BotAPI.AvailableTypes;

namespace LifeTimeBot.Services;

public class UserUtcService
{
    private readonly BotDbContext _db;
    
    public UserUtcService(BotDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetUserUtcOffset(long botId, long userId)
    {
        BotUserEntity? user = await _db.Users.FirstOrDefaultAsync(u => u.BotId == botId && u.TelegramId == userId);

        if (user == null) return 0;
        
        if (user.AdditionalProperties.Contains(AppConstants.UserUtcPropKey) == false)
        {
            return 0;
        }
        
        return user.AdditionalProperties.Get<int>(AppConstants.UserUtcPropKey);
    }
}