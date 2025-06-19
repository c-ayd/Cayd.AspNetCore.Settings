namespace Cayd.AspNetCore.Settings.Test.Integration.Utilities
{
    public class MyUserSecrets : ISettings
    {
        public static string SettingsKey => "MySecret";

        public required string SecretValue { get; set; }
    }
}
