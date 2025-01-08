using FastEndpoints;

namespace LifeTimeBot.Endpoints.Ai.GetDailyAnalysis;

public sealed class AiGroup : Group
{
    public AiGroup()
    {
        Configure("ai", c =>
        {
            //c.Description(d => d.WithTags("user"));
        });
    }
}