using System.Text.Json;
using HuggingFace;
using LifeTimeBot.Models;
using LifeTimeBot.Services.LLM.Dto;

namespace LifeTimeBot.Services.LLM;

public partial class LlmService
{
    /// <summary>
    /// Для текста определяет активность.
    /// Извлекает дату начала, окончания, описание активности.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<LlmActivityDataResult> GetActivityDataFromText(string text)
    {
        string answer = await GetAnswerFromLLM(_llmOptions.GetActivityPromptTemplate, text);
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