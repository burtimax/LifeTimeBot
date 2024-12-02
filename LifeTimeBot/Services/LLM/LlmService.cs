using HuggingFace;
using System.Text;
using System.Text.Json;
using LifeTimeBot.App.Options;
using Microsoft.Extensions.Options;

namespace LifeTimeBot.Services.LLM;

public partial class LlmService
{
    private readonly LlmServiceOptions _llmOptions;
    private HttpClient client = new HttpClient();
    
    public LlmService(IOptions<AppOptions> options)
    {
        _llmOptions = options.Value.LlmHuggingFaceService;
    }


    public async Task<string> GetAnswerFromLLM(string prompt, string userText)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _llmOptions.Url);
        request.Headers.Add("accept", "*/*");
        request.Headers.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("authorization", $"Bearer {_llmOptions.ApiKey}");
        // request.Headers.Add("content-type", "application/json");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Mobile Safari/537.36");

        LlmRequestData requestData = new()
        {
            Model = _llmOptions.Model,
            Temperature = _llmOptions.Temperature,
            MaxTokens = _llmOptions.MaxTokens,
            TopP = _llmOptions.TopP,
        };

        requestData.Messages = new()
        {
            new()
            {
                Role = "system",
                Content = prompt
            },
            new()
            {
                Role = "user",
                Content = userText
            }
        };

        string json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Content = content;
        var responseData = await client.SendAsync(request);
        responseData.EnsureSuccessStatusCode();
        string responseJson = await responseData.Content.ReadAsStringAsync();
        ChatCompletionResponse? response = JsonSerializer.Deserialize<ChatCompletionResponse>(responseJson);
        if (response is not null && response?.Choices is not null && response.Choices.Any())
        {
            return response.Choices.FirstOrDefault().Message?.Content ?? null;
        }

        return null;
    }
}