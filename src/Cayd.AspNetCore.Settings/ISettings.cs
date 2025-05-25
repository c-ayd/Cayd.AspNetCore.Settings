namespace Cayd.AspNetCore.Settings
{
    /// <summary>
    /// Marks classes representing settings structures to be added automatically.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Top-level section name of the settings.
        /// </summary>
        abstract static string SettingsKey { get; }
    }
}
