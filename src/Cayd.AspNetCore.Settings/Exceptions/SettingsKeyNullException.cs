namespace Cayd.AspNetCore.Settings.Exceptions
{
    internal class SettingsKeyNullException : Exception
    {
        internal SettingsKeyNullException(string typeName)
            : base($"The settings key of {typeName} is null.") { }
    }
}
