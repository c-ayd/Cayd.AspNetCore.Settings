namespace Cayd.AspNetCore.Settings.Test.Integration.Utilities
{
    public class Settings1 : ISettings
    {
        public static string SettingsKey => "MyGroupedSettings:Settings1";

        public required string StrValue { get; set; }
    }
}
