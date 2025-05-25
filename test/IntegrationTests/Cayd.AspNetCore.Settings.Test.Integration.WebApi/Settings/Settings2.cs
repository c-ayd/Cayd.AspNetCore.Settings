namespace Cayd.AspNetCore.Settings.Test.Integration.WebApi.Settings
{
    public class Settings2 : ISettings
    {
        public static string SettingsKey => "MyGroupedSettings:Settings2";

        public required int IntValue { get; set; }
    }
}
