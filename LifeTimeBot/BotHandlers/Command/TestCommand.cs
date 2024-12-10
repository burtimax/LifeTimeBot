using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LifeTimeBot.App.Constants;
using LifeTimeBot.BotHandlers.State;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.LLM;
using MultipleBotFramework;
using MultipleBotFramework.Attributes;
using MultipleBotFramework.Constants;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Extensions;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.GettingUpdates;

namespace MetalBoardBot.BotHandlers.Command;

[BotHandler(command: "/test", requiredUserClaims: new []{BotConstants.BaseBotClaims.IAmBruceAlmighty})]
public class TestCommand : BaseLifeTimeBotHandler
{
    private readonly ActivityService _activityService;
    private readonly LlmService _llmService;
    
    public TestCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _activityService = serviceProvider.GetRequiredService<ActivityService>();
        _llmService = serviceProvider.GetRequiredService<LlmService>();
    }

    public override async Task HandleBotRequest(Update update)
    {
        UserProfilePhotos photos = await BotClient.GetUserProfilePhotosAsync(User.TelegramId);
        IEnumerable<PhotoSize> photo = photos.Photos.First();
        string json = JsonSerializer.Serialize(photo);
        await Answer(json, parseMode: ParseMode.Html);
    }
}