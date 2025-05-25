namespace Cayd.AspNetCore.Settings
{
    public interface ISettings
    {
        abstract static string SettingsKey { get; }
    }
}
