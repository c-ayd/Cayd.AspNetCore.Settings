namespace Cayd.AspNetCore.Settings
{
    /// <summary>
    /// Marks the class as a representation of a configuration that will be added automatically.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Top-level section name of the configuration.
        /// </summary>
        abstract static string SettingsKey { get; }
    }
}
