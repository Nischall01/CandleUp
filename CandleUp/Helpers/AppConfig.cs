namespace CandleUp.Helpers;

public static class AppConfig
{
    private static string DataFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CandleUp");

    public static string DbFile => Path.Combine(DataFolder, "birthdays.json");
    public static string SettingsFile => Path.Combine(DataFolder, "settings.json");
    public static string LogFile => Path.Combine(DataFolder, "logs.txt");

    public static void EnsureDataFolder()
    {
        if (!Directory.Exists(DataFolder))
            Directory.CreateDirectory(DataFolder);
    }
}