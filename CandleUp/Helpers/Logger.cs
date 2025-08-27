namespace CandleUp.Helpers;

public static class Logger
{
    public static void Log(string errorMessage)
    {
        var logPath = AppConfig.LogFile;
        File.WriteAllText(logPath, errorMessage);
    }
}