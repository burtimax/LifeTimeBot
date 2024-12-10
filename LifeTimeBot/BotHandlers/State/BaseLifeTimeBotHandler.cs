using LifeTimeBot.App.Constants;
using LifeTimeBot.App.Options;
using LifeTimeBot.Resources;
using Microsoft.Extensions.Options;
using MultipleBotFramework.Base;
using MultipleBotFramework.Constants;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State;

public class BaseLifeTimeBotHandler : BaseBotHandler
{
    protected readonly BotResources R;
    private readonly AppOptions AppConfig;
    
    public BaseLifeTimeBotHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        R = serviceProvider.GetRequiredService<IOptions<BotResources>>().Value;
        AppConfig = serviceProvider.GetRequiredService<IOptions<AppOptions>>().Value;
    }

    /// <summary>
    /// Получить настройку пользователя UTC.
    /// </summary>
    /// <returns></returns>
    protected int? GetUserUtc()
    {
        if (User.AdditionalProperties.Contains(AppConstants.UserUtcPropKey) == false)
        {
            return null;
        }
        
        return User.AdditionalProperties.Get<int>(AppConstants.UserUtcPropKey);
    }
    
    protected async Task SetUserUtc(int utcOffset)
    {
        User.AdditionalProperties.Set(AppConstants.UserUtcPropKey, utcOffset);
        BotDbContext.Update(User);
        await BotDbContext.SaveChangesAsync();
    }
}