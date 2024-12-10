using FastEndpoints;

namespace LifeTimeBot.Endpoints.Reports;

public sealed class ReportGroup : Group
{
    public ReportGroup()
    {
        Configure("report", c =>
        {
            //c.Description(d => d.WithTags("user"));
        });
    }
}