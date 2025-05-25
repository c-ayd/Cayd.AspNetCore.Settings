using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        public static void AddSettingsFromAssembly(this IHostApplicationBuilder builder, Assembly assembly)
        {
            AddSettings(builder, assembly);
        }
    }
}
