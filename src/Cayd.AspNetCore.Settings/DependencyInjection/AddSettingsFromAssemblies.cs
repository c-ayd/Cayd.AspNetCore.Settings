using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        /// <summary>
        /// Finds all classes inheriting <see cref="ISettings"/> by the given assemblies and configures the settings automatically.
        /// </summary>
        /// <param name="assemblies">Assemblies in which settings will be searched.</param>
        public static void AddSettingsFromAssemblies(this IHostApplicationBuilder builder, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddSettings(builder, assembly);
            }
        }
    }
}
