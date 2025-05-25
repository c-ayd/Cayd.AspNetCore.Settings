namespace Cayd.AspNetCore.Settings
{
    /// <summary>
    /// Marks classes representing configuration structures to be added automatically.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Top-level section name of the configuration.
        /// </summary>
        abstract static string SettingsKey { get; }
    }
}
