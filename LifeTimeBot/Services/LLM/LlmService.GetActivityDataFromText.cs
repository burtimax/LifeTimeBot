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
    public async Task<LlmActivityDataResult> GetActivityDataFromText(string text, string lastActivityTimeStr = null)
    {
        string prompt = _llmOptions.GetActivityPromptTemplate.Replace("{last_end_time}", lastActivityTimeStr);
        string answer = await GetAnswerFromLLM(prompt, text);

        List<string> activitiesStr = answer.Replace("},", "}|").Split('|').ToList();

        List<ActivityModel>? activities = new List<ActivityModel>();
        
        foreach (var ac in activitiesStr)
        {
            if (ac.Trim(' ', '\n').StartsWith("{"))
            {
                activities.Add(JsonSerializer.Deserialize<ActivityModel>(ac));
            }
        }
        
        if (activities.Any() == false)
        {
            return new() { LlmResponse = answer };
        }
        
        return new() { Activities = activities };
    }
}