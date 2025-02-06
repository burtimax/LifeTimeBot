using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Services;
using LifeTimeBot.Services.ASR;
using LifeTimeBot.Services.LLM;
using LifeTimeBot.Services.LLM.Dto;
using MultipleBotFramework.Dispatcher.HandlerResolvers;
using MultipleBotFramework.Enums;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Extensions.ITelegramApiClient;
using MultipleBotFramework.Models;
using MultipleBotFramework.Utils.Keyboard;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.UpdatingMessages;

namespace LifeTimeBot.BotHandlers.State.Main;

[BotHandler(stateName:Name, version: 2.0)]
public partial class MainState: BaseLifeTimeBotHandler
{
    public const string Name = "MainState";

    private Message? _mesInProgress = null;
    private bool _mesExistedInProgress = true;
    
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
            await HandleVoiceMessage(message);
        }
        
        if (message.Type() == MessageType.Text)
        {
            await HandleTextMessage(message.Text);
        }
    }
    
    private async Task HandleVoiceMessage(Message message)
    {
        var voice = message.Voice;
        if (voice.Duration > 40)
        {
            await Answer(R.TooLongVoice);
            return;
        }

        _mesInProgress = await Answer(R.InProgress);
        _mesExistedInProgress = true;

        string? recognisedText = await GetTextFromVoice(voice);
        if (recognisedText == null) return;

        await HandleTextMessage(recognisedText);
        
        return;
    }
    
    private async Task HandleTextMessage(string text)
    {
        if (_mesInProgress == null)
        {
            _mesInProgress = await Answer(R.InProgress);
            _mesExistedInProgress = true;
        }
        
        
        
        await ProcessActivities(text);
        
        return;
    }


    private async Task GetTextDataInfo(string text)
    {
        
    }
}