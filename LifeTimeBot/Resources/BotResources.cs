namespace LifeTimeBot.Resources;

public partial class BotResources
{
    public string Introduction { get; set; }
    public string MainStateIntroduction { get; set; }
    public string DontUnderstandYou { get; set; }
    public string TooLongVoice { get; set; }
    public string CannotRecognizeTextInVoice { get; set; }
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
}