namespace Cayd.AspNetCore.Settings.Test.Integration.Utilities
{
    public class MySettings : ISettings
    {
        public static string SettingsKey => "MySettings";

        public required string StrValue { get; set; }
        public required int IntValue { get; set; }
        public required List<string> ListStr { get; set; }
        public required List<int> ListInt { get; set; }
    }
}
