using LifeTimeBot.Db.AppDb.Entities;
using LifeTimeBot.Resources.Nested;

namespace LifeTimeBot.Resources;

public partial class BotResources
{
    public AppResources AppResources { get; set; }
    public string Introduction { get; set; }
    public string MainStateIntroduction { get; set; }
    public string DontUnderstandYou { get; set; }
    public string TooLongVoice { get; set; }
    public string CannotRecognizeTextInVoice { get; set; }
    public string NotFoundActivityInText { get; set; }
    public string NotFoundActivityInVoice { get; set; }
    public string ActivityTemplate { get; set; }
    public string ActivityListItemTemplate { get; set; }
    public string InProgress { get; set; }
    public string BtnConfirmActivity { get; set; }
    public string BtnConfirmActivityKey = "activ_confirm:";
    public string BtnConfirmActivityCallback(long activityId) => $"{BtnConfirmActivityKey}{activityId}";
    public string BtnCancelActivity { get; set; }
    public string BtnCancelActivityKey = "activ_cancel:";
    public string BtnCancelActivityCallback(long activityId) => $"{BtnCancelActivityKey}{activityId}";
    public string ChooseUtc { get; set; }
    public string BtnChooseUtcKey = "choose_utc:";
    public string BtnChooseUtcCallback(int utcDifference) => $"{BtnChooseUtcKey}{utcDifference}";
    public string UserChosenUtc { get; set; }
    public string NoActivitiesToday { get; set; }
    public int MinActivitiesCountForAiDailyRecommendation { get; set; }
    public string NotEnoughActivityForAiDailyRecommendation { get; set; }
    public string NoRecommendationsForDaily { get; set; }
    public string AddBalanceTypeToActivityKey = "add_bal_t_ac:";
    public string AddBalanceTypeToActivityCallback(long activityId, BalanceType type) => $"{AddBalanceTypeToActivityKey}{activityId}:{(int)type}";
    public string RemoveBalanceTypeFromActivityKey = "rem_bal_t_ac:";
    public string RemoveBalanceTypeFromActivityCallback(long activityId, BalanceType type) => $"{RemoveBalanceTypeFromActivityKey}{activityId}:{(int)type}";
    public string BtnBalanceContainsNameFormat { get; set; }
    public string BtnBalanceNotContainsNameFormat { get; set; }
    public string BtnOpenBalanceTypes { get; set; }
    public string BtnOpenBalanceTypesKey = "open_balance_types:";
    public string BtnOpenBalanceTypesKeyCallback(long activityId) => $"{BtnOpenBalanceTypesKey}{activityId}";
}