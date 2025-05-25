using Cayd.AspNetCore.Settings.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        private static void AddSettings(IHostApplicationBuilder builder, Assembly assembly)
        {
            var settingsTypes = assembly.GetTypes()
                .Where(t => typeof(ISettings).IsAssignableFrom(t) && t.IsClass)
                .ToList();

            foreach (var st in settingsTypes)
            {
                var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods()
                    .First(m => m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure))
                    .MakeGenericMethod(st);

                var key = (string?)st.GetProperty(nameof(ISettings.SettingsKey))?.GetValue(null);
                if (key == null)
                    throw new SettingsKeyNullException(st.Name);

                configureMethod.Invoke(null, new object[] { builder.Services, builder.Configuration.GetSection(key) });
            }
        }
    }
}
