using System.Globalization;
using FastEndpoints;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Extensions;
using LifeTimeBot.Resources;
using LifeTimeBot.Resources.Nested;
using LifeTimeBot.Services;
using Microsoft.Extensions.Options;

namespace LifeTimeBot.Endpoints.Daily.GetResources;

sealed class GetResourcesResponse 
{
    public Dictionary<string, string> BalanceTypes { get; set; }
    public Dictionary<string, string> ActivityTypes { get; set; }
}

sealed class GetResourcesEndpoint : EndpointWithoutRequest<GetResourcesResponse>
{
    private AppResources _r;

    public GetResourcesEndpoint(IOptions<BotResources> options)
    {
        _r = options.Value.AppResources;
    }

    public override void Configure()
    {
        Get("/get");
        AllowAnonymous();
        Group<ResourcesGroup>();
        Summary(s =>
        {
            s.Summary = "Ресурсы приложения";
            s.Description = $"Ресурсы приложения";
        });
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var at = _r.ActivityTypes;
        var bt = _r.BalanceTypes;

        string getAt (ActivityType at) => ((int)at).ToString();
        string getBt (BalanceType bt) => ((int)bt).ToString();

        GetResourcesResponse response = new()
        {
            ActivityTypes = new Dictionary<string, string>()
            {
                { getAt(ActivityType.None), at.None },
                { getAt(ActivityType.Sleep), at.Sleep },
                { getAt(ActivityType.Sport), at.Sport },
                { getAt(ActivityType.Work), at.Work },
                { getAt(ActivityType.Education), at.Education },
                { getAt(ActivityType.Relax), at.Relax },
            },
            BalanceTypes = new Dictionary<string, string>()
            {
                { getBt(BalanceType.None), bt.None },
                { getBt(BalanceType.СareerWorkBusiness), bt.CareerWorkBusiness },
                { getBt(BalanceType.LoveFamilyChildren), bt.LoveFamilyChildren },
                { getBt(BalanceType.HealthyAndSport), bt.HealthyAndSport },
                { getBt(BalanceType.FriendsAndCommunity), bt.FriendsAndCommunity },
                { getBt(BalanceType.HobbyAndBrightnessOfLife), bt.HobbyAndBrightnessOfLife },
                { getBt(BalanceType.PersonalDevelopmentAndEducation), bt.PersonalDevelopmentAndEducation },
                { getBt(BalanceType.Finance), bt.Finance },
                { getBt(BalanceType.Spirituality), bt.Spirituality },
            }
        };

        await SendAsync(response);
    }
}