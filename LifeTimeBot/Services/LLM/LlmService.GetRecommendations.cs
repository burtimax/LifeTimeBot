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
    /// Дает рекомендации по улучшению распорядка дня
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<string?> GetRecommendationsForRoutine(string dailyRoutine)
    {
        string answer = await GetAnswerFromLLM(_llmOptions.GetRecommendationsForRoutinePromptTemplate, dailyRoutine);
        return answer;
    }
    
    public async Task<string?> GetRecommendationsForActivities(List<ActivityEntity> activities)
    {
        string dailyRoutine = ActivitiesToString(activities);
        return await GetRecommendationsForRoutine(dailyRoutine);
    }
    
    private string ActivitiesToString(List<ActivityEntity> activities)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var ac in activities)
        {
            sb.AppendLine(string.Format("{0}-{1}: {2}", ac.StartTime.Value.ToString(AppConstants.TimeFormat), ac.EndTime.Value.ToString(AppConstants.TimeFormat), ac.Description));
        }

        return sb.ToString();
    }
}