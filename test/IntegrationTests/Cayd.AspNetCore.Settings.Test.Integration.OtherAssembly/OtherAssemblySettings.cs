namespace Cayd.AspNetCore.Settings.Test.Integration.OtherAssembly
{
    public class OtherAssemblySettings : ISettings
    {
        public static string SettingsKey => "OtherAssembly";

        public required string Value { get; set; }
    }
}
