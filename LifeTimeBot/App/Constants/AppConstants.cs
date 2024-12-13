namespace LifeTimeBot.App.Constants;

public static partial class AppConstants
{
    public const string UserUtcPropKey = "utc_tz";
    public const string DateTimeFormat = "dd.MM.yyyy HH:mm";
    public const string TimeFormat = "HH:mm";

    public static class HttpClients
    {
        public const string AsrClient = "asr_client";
        public const string LlmClient = "llm_client";
    }
    
    public static List<UTCLabel> UTCTimezones = new List<UTCLabel>()
    {
        new UTCLabel() { Difference = -12, Title = "UTC-12:00" },
        new UTCLabel() { Difference = -11, Title = "UTC-11:00" },
        new UTCLabel() { Difference = -10, Title = "UTC-10:00" },
        new UTCLabel() { Difference = -9, Title = "UTC-09:00" },
        new UTCLabel() { Difference = -8, Title = "UTC-08:00" },
        new UTCLabel() { Difference = -7, Title = "UTC-07:00" },
        new UTCLabel() { Difference = -6, Title = "UTC-06:00" },
        new UTCLabel() { Difference = -5, Title = "UTC-05:00" },
        new UTCLabel() { Difference = -4, Title = "UTC-04:00" },
        new UTCLabel() { Difference = -3, Title = "UTC-03:00" },
        new UTCLabel() { Difference = -2, Title = "UTC-02:00" },
        new UTCLabel() { Difference = -1, Title = "UTC-01:00" },
        new UTCLabel() { Difference = 0, Title = "UTC±00:00" },
        new UTCLabel() { Difference = 1, Title = "UTC+01:00" },
        new UTCLabel() { Difference = 2, Title = "UTC+02:00" },
        new UTCLabel() { Difference = 3, Title = "UTC+03:00" },
        new UTCLabel() { Difference = 4, Title = "UTC+04:00" },
        new UTCLabel() { Difference = 5, Title = "UTC+05:00" },
        new UTCLabel() { Difference = 6, Title = "UTC+06:00" },
        new UTCLabel() { Difference = 7, Title = "UTC+07:00" },
        new UTCLabel() { Difference = 8, Title = "UTC+08:00" },
        new UTCLabel() { Difference = 9, Title = "UTC+09:00" },
        new UTCLabel() { Difference = 10, Title = "UTC+10:00" },
        new UTCLabel() { Difference = 11, Title = "UTC+11:00" },
        new UTCLabel() { Difference = 12, Title = "UTC+12:00" },
    };
}

public class UTCLabel
{
    public int Difference { get; set; }
    public string Title { get; set; }
}