using LifeTimeBot.Db.AppDb.Entities;

namespace LifeTimeBot.Resources.Nested;

public class AppResources
{
    public BalanceTypesResources BalanceTypes { get; set; }
    public ActivityTypesResources ActivityTypes { get; set; }

    public string GetBalanceTypeName(BalanceType type)
        => type switch
        {
            BalanceType.None => BalanceTypes.None,
            BalanceType.СareerWorkBusiness => BalanceTypes.CareerWorkBusiness,
            BalanceType.LoveFamilyChildren => BalanceTypes.LoveFamilyChildren,
            BalanceType.HealthyAndSport => BalanceTypes.HealthyAndSport,
            BalanceType.FriendsAndCommunity => BalanceTypes.FriendsAndCommunity,
            BalanceType.HobbyAndBrightnessOfLife => BalanceTypes.HobbyAndBrightnessOfLife,
            BalanceType.PersonalDevelopmentAndEducation => BalanceTypes.PersonalDevelopmentAndEducation,
            BalanceType.Finance => BalanceTypes.Finance,
            BalanceType.Spirituality => BalanceTypes.Spirituality
        };
    
    public string GetActivityTypeName(ActivityType type)
        => type switch
        {
            ActivityType.None => ActivityTypes.None,
            ActivityType.Sleep => ActivityTypes.Sleep,
            ActivityType.Sport => ActivityTypes.Sport,
            ActivityType.Work => ActivityTypes.Work,
            ActivityType.Education => ActivityTypes.Education,
            ActivityType.Relax => ActivityTypes.Relax,
            
        };
}

public class ActivityTypesResources
{
    public string None { get; set; }
    public string Sleep { get; set; }
    public string Sport { get; set; }
    public string Work { get; set; }
    public string Education { get; set; }
    public string Relax { get; set; }
}

public class BalanceTypesResources
{
    public string None { get; set; }
    public string CareerWorkBusiness { get; set; }
    public string LoveFamilyChildren { get; set; }
    public string HealthyAndSport { get; set; }
    public string FriendsAndCommunity { get; set; }
    public string HobbyAndBrightnessOfLife { get; set; }
    public string PersonalDevelopmentAndEducation { get; set; }
    public string Finance { get; set; }
    public string Spirituality { get; set; }
}