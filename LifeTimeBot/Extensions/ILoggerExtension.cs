using LifeTimeBot.App.Constants;

namespace LifeTimeBot.Extensions;

public static class ILoggerExtension
{
    public static void LogEndpointCall<T>(this ILogger<T> logger, string endpoint)
        => logger.LogDebug(string.Format(AppConstants.Logging.CallEndpointFormat, endpoint));
}