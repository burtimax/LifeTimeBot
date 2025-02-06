using System.Text;
using System.Text.Json;
using HuggingFace;
using LifeTimeBot.App.Constants;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Models;
using LifeTimeBot.Services.LLM.Dto;

namespace LifeTimeBot.Services.LLM;

public partial class LlmService
{
    /// <summary>
    /// Получить информацию о том, что содержится в тексте: задачи, активности или прочее?
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<string?> GetTextDataInfo(string text)
    {
        string answer = await GetAnswerFromLLM2(_llmOptions.GetRecommendationsForRoutinePromptTemplate, text);
        return answer;
    }
}