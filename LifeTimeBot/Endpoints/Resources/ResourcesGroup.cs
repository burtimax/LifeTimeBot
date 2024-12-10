using FastEndpoints;

namespace LifeTimeBot.Endpoints.Daily;

public sealed class ResourcesGroup : Group
{
    public ResourcesGroup()
    {
        Configure("resources", c =>
        {
            //c.Description(d => d.WithTags("user"));
        });
    }
}