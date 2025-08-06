namespace Cayd.AspNetCore.Settings.Test.Integration.Utilities
{
    public class EnvSettings : ISettings
    {
        public static string SettingsKey => "Env";

        public required string StrValue { get; set; }
        public required int IntValue { get; set; }
        public required List<string> StrValues { get; set; }
        public required List<int> IntValues { get; set; }
    }
}
