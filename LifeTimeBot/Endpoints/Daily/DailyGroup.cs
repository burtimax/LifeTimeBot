using FastEndpoints;

namespace LifeTimeBot.Endpoints.Daily;

public sealed class DailyGroup : Group
{
    public DailyGroup()
    {
        Configure("daily", c =>
        {
            //c.Description(d => d.WithTags("user"));
        });
    }
}