using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        /// <summary>
        /// Finds all classes implementing <see cref="ISettings"/> by the given assembly and configures the settings automatically.
        /// </summary>
        /// <param name="assembly">Assembly in which settings will be searched.</param>
        public static void AddSettingsFromAssembly(this IHostApplicationBuilder builder, Assembly assembly)
        {
            AddSettings(builder, assembly);
        }
    }
}
