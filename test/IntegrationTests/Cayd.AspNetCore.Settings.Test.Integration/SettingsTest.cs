using Cayd.AspNetCore.Settings.DependencyInjection;
using Cayd.AspNetCore.Settings.Test.Integration.OtherAssembly;
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
        private readonly Assembly _otherAssembly;

        private readonly List<string> _listStr;
        private readonly List<int> _listInt;

        public SettingsTest()
        {
            _currentAssembly = Assembly.GetAssembly(typeof(SettingsTest))!;
            _otherAssembly = Assembly.GetAssembly(typeof(OtherAssemblySettings))!;

            _listStr = new List<string>() { "1", "2", "3" };
            _listInt = new List<int>() { 1, 2, 3 };
        }

        private async Task<IHost> CreateHost(ERegistrationType registrationType, params Assembly[] assemblies)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Development,
                ContentRootPath = Directory.GetCurrentDirectory()
            });

            builder.Configuration
                .AddJsonFile("Utilities/appsettings.json", false)
                .AddUserSecrets<SettingsTest>();

            AddSettings(builder, registrationType, assemblies);

            builder.WebHost.UseTestServer();

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }

        private void AddSettings(WebApplicationBuilder builder, ERegistrationType registrationType, params Assembly[] assemblies)
        {
            if (assemblies.Length == 1)
            {
                switch (registrationType)
                {
                    case ERegistrationType.Services:
                        builder.Services.AddSettingsFromAssembly(builder.Configuration, assemblies[0]);
                        break;
                    case ERegistrationType.Builder:
                    default:
                        builder.AddSettingsFromAssembly(assemblies[0]);
                        break;
                }
            }
            else
            {
                switch (registrationType)
                {
                    case ERegistrationType.Services:
                        builder.Services.AddSettingsFromAssemblies(builder.Configuration, assemblies);
                        break;
                    case ERegistrationType.Builder:
                    default:
                        builder.AddSettingsFromAssemblies(assemblies);
                        break;
                }
            }
        }

        private async Task DisposeHost(IHost host)
        {
            await host.StopAsync();
            host.Dispose();
        }

        [Theory]
        [InlineData(ERegistrationType.Builder)]
        [InlineData(ERegistrationType.Services)]
        public async Task WhenClassesInSameAssemblyImplementISettingsInterfaceAndAreRegistered_ShouldReturnValuesInSettings(ERegistrationType registrationType)
        {
            // Arrange
            var host = await CreateHost(registrationType, _currentAssembly);

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

        [Theory]
        [InlineData(ERegistrationType.Builder)]
        [InlineData(ERegistrationType.Services)]
        public async Task WhenClassesInDifferentAssembliesImplementISettingsInterfaceAndAreRegistered_ShouldReturnValuesInSettings(ERegistrationType registrationType)
        {
            // Arrange
            var host = await CreateHost(registrationType, _currentAssembly, _otherAssembly);

            // Act
            var mySettings = host.Services.GetRequiredService<IOptions<MySettings>>().Value;
            var settings1 = host.Services.GetRequiredService<IOptions<Settings1>>().Value;
            var settings2 = host.Services.GetRequiredService<IOptions<Settings2>>().Value;
            var myUserSecrets = host.Services.GetRequiredService<IOptions<MyUserSecrets>>().Value;
            var otherAssemblySettings = host.Services.GetRequiredService<IOptions<OtherAssemblySettings>>().Value;

            // Assert
            Assert.Equal("test-value", mySettings.StrValue);
            Assert.Equal(5, mySettings.IntValue);
            Assert.Equal(_listStr, mySettings.ListStr);
            Assert.Equal(_listInt, mySettings.ListInt);

            Assert.Equal("value", settings1.StrValue);

            Assert.Equal(-10, settings2.IntValue);

            Assert.Equal("test-secret-value", myUserSecrets.SecretValue);

            Assert.Equal("other-assembly-value", otherAssemblySettings.Value);

            await DisposeHost(host);
        }

        public enum ERegistrationType
        {
            Builder = 0,
            Services = 1
        }
    }
}
