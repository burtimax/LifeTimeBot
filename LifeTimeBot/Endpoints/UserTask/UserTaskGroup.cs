using FastEndpoints;

namespace LifeTimeBot.Endpoints.UserTask;

public sealed class UserTaskGroup : Group
{
    public UserTaskGroup()
    {
        Configure("user-task", c =>
        {
            //c.Description(d => d.WithTags("task"));
        });
    }
} 