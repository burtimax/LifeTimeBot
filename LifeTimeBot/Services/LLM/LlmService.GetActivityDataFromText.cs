using System.Text.Json;
using HuggingFace;
using LifeTimeBot.Models;

namespace LifeTimeBot.Services.LLM;

public partial class LlmService
{
    public async Task<LlmActivityDataResult> GetActivityDataFromText(string text)
    {
        string answer = await GetAnswerFromLLM(_llmOptions.PromptTemplate, text);
        ActivityModel? activity = null;
        if (answer.Trim(' ').StartsWith("{"))
        {
            activity = JsonSerializer.Deserialize<ActivityModel>(answer);
        }

        if (activity is null)
        {
            return new() { LlmResponse = answer };
        }
        
        return new() { Activity = activity };
    }
}