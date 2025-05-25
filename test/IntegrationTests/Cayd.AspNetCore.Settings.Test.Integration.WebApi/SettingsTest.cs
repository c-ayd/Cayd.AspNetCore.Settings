using Cayd.AspNetCore.Settings.DependencyInjection;
using Cayd.AspNetCore.Settings.Test.Integration.WebApi.Settings;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Cayd.AspNetCore.Settings.Test.Integration.WebApi
{
    public class SettingsTest
    {
        private readonly Assembly _currentAssembly;

        public SettingsTest()
        {
            _currentAssembly = Assembly.GetAssembly(typeof(SettingsTest))!;
        }

        [Fact]
        public void WhenClassesImplementISettingsInterfaceAndRegisteredViaAssembly_ShouldReturnValuesInSettings()
        {
            // Arrange
            var listStr = new List<string>() { "1", "2", "3" };
            var listInt = new List<int>() { 1, 2, 3 };

            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddUserSecrets(_currentAssembly);
            
            // Act
            builder.AddSettingsFromAssembly(_currentAssembly);
            var app = builder.Build();

            // Assert
            var mySettings = app.Services.GetRequiredService<IOptions<MySettings>>().Value;
            var settings1 = app.Services.GetRequiredService<IOptions<Settings1>>().Value;
            var settings2 = app.Services.GetRequiredService<IOptions<Settings2>>().Value;
            var mySecretSettings = app.Services.GetRequiredService<IOptions<MySecretSettings>>().Value;

            Assert.Equal("test-value", mySettings.StrValue);
            Assert.Equal(42, mySettings.IntValue);
            Assert.Equal(listStr, mySettings.ListStr);
            Assert.Equal(listInt, mySettings.ListInt);

            Assert.Equal("value", settings1.StrValue);

            Assert.Equal(-42, settings2.IntValue);

            Assert.Equal("test-secret-value", mySecretSettings.SecretValue);
        }
    }
}
