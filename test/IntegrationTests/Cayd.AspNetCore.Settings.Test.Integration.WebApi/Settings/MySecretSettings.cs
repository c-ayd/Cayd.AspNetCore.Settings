namespace Cayd.AspNetCore.Settings.Test.Integration.WebApi.Settings
{
    public class MySecretSettings : ISettings
    {
        public static string SettingsKey => "MySecret";

        public required string SecretValue { get; set; }
    }
}
