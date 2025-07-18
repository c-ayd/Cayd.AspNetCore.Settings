using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        /// <summary>
        /// Finds all classes implementing <see cref="ISettings"/> by the given assembly and configures the settings automatically.
        /// </summary>
        /// <param name="assembly">Assembly in which settings will be searched.</param>
        /// <param name="configuration">Configuration instance in which settings are defined.</param>
        public static void AddSettingsFromAssembly(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            AddSettings(services, configuration, assembly);
        }
    }
}
