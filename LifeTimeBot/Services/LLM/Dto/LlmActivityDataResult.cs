using LifeTimeBot.Models;

namespace LifeTimeBot.Services.LLM.Dto;

public class LlmActivityDataResult
{
    public List<ActivityModel>? Activities { get; set; }
    public string LlmResponse { get; set; }
}