using LifeTimeBot.App.Options;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;

namespace LifeTimeBot.Services.ASR;
using AssemblyAI;
using AssemblyAI.Transcripts;

public class AsrService
{
    private AsrServiceOptions _asrOptions;
    public AsrService(IOptions<AppOptions> options)
    {
        _asrOptions = options.Value.AsrService;
    }

  
    public async Task<string?> VoiceToText(byte[] bytes)
    {
        var client = new AssemblyAIClient(_asrOptions.AssemblyAiKey);

        string text = null;
        
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            Transcript res = await client.Transcripts.TranscribeAsync(ms, new TranscriptOptionalParams()
            {
                LanguageCode = TranscriptLanguageCode.Ru,
        
            });
            text = res.Text ?? null;
        }

        return text;
    }
}