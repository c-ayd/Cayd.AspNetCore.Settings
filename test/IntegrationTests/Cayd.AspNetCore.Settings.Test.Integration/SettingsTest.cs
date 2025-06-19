using Cayd.AspNetCore.Settings.DependencyInjection;
using Cayd.AspNetCore.Settings.Test.Integration.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.Test.Integration
{
    public class SettingsTest
    {
        private readonly Assembly _currentAssembly;

        private readonly List<string> _listStr;
        private readonly List<int> _listInt;

        public SettingsTest()
        {
            _currentAssembly = Assembly.GetAssembly(typeof(SettingsTest))!;

            _listStr = new List<string>() { "1", "2", "3" };
            _listInt = new List<int>() { 1, 2, 3 };
        }

        private async Task<IHost> CreateHost()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Development,
                ContentRootPath = Directory.GetCurrentDirectory()
            });

            builder.Configuration
                .AddJsonFile("Utilities/appsettings.json", false)
                .AddUserSecrets<SettingsTest>();

            builder.AddSettingsFromAssembly(_currentAssembly);
            builder.WebHost.UseTestServer();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private async Task DisposeHost(IHost host)
        {
            await host.StopAsync();
            host.Dispose();
        }

        [Fact]
        public async Task WhenClassesImplementISettingsInterfaceAndRegisteredViaAssembly_ShouldReturnValuesInSettings()
        {
            // Arrange
            var host = await CreateHost();

            // Act
            var mySettings = host.Services.GetRequiredService<IOptions<MySettings>>().Value;
            var settings1 = host.Services.GetRequiredService<IOptions<Settings1>>().Value;
            var settings2 = host.Services.GetRequiredService<IOptions<Settings2>>().Value;
            var myUserSecrets = host.Services.GetRequiredService<IOptions<MyUserSecrets>>().Value;

            // Assert
            Assert.Equal("test-value", mySettings.StrValue);
            Assert.Equal(5, mySettings.IntValue);
            Assert.Equal(_listStr, mySettings.ListStr);
            Assert.Equal(_listInt, mySettings.ListInt);

            Assert.Equal("value", settings1.StrValue);

            Assert.Equal(-10, settings2.IntValue);

            Assert.Equal("test-secret-value", myUserSecrets.SecretValue);

            await DisposeHost(host);
        }
    }
}
