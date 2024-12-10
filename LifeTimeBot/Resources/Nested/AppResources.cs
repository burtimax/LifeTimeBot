namespace LifeTimeBot.Resources.Nested;

public class AppResources
{
    public BalanceTypesResources BalanceTypes { get; set; }
    public ActivityTypesResources ActivityTypes { get; set; }
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