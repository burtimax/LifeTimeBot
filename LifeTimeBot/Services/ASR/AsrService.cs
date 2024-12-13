using LifeTimeBot.App.Constants;
using LifeTimeBot.App.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Telegram.BotAPI;

namespace LifeTimeBot.Services.ASR;
using AssemblyAI;
using AssemblyAI.Transcripts;

public class AsrService
{
    private AsrServiceOptions _asrOptions;
    private HttpClient whisperClient;
    public AsrService(IOptions<AppOptions> options, IHttpClientFactory clientFactory)
    {
        this.whisperClient = clientFactory.CreateClient(AppConstants.HttpClients.AsrClient);
        _asrOptions = options.Value.AsrService;
    }

  
    public async Task<string?> AssemblyVoiceToText(byte[] bytes)
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
    
    public async Task<string?> WhisperVoiceToText(byte[] bytes)
    {
        try
        {
            whisperClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_asrOptions.WhisperToken}");

            using (var content = new ByteArrayContent(bytes))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/ogg");
                
                HttpResponseMessage response = await whisperClient.PostAsync(_asrOptions.WhisperUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    AsrResultDto? asrResult = JsonConvert.DeserializeObject<AsrResultDto>(result);
                    return asrResult?.Text;
                }
                else
                {
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorDetails);
                }
            }
            
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}