using LifeTimeBot.Models;

namespace LifeTimeBot.Services.LLM.Dto;

public class LlmActivityDataResult
{
    public ActivityModel? Activity { get; set; }
    public string LlmResponse { get; set; }
}