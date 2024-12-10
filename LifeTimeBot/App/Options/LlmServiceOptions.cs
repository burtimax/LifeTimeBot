namespace LifeTimeBot.App.Options;

public class LlmServiceOptions
{
    public string Url { get; set; }
    public string ApiKey { get; set; }
    public string Model { get; set; }
    public double Temperature { get; set; }
    public double TopP { get; set; }
    public int MaxTokens { get; set; }
    public string GetActivityPromptTemplate { get; set; }
    public string GetRecommendationsForRoutinePromptTemplate { get; set; }
}