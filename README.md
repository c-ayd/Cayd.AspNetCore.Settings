## About
This package helps to register configurations automatically from appsettings, user secrets and environment variables into the ASP.NET Core dependency injection system using `IOptions<T>`.

## How to Use
After installing the package, you can use `ISettings` interface in the `Cayd.AspNetCore.Settings` namespace to mark your settings classes for the automatic registering.

For example, suppose you have JWT configuration like below and want to use it in your code via `IOptions<T>`.
```json
{
  "Jwt": {
    "SecretKey": "...",
    "Issuer": "api.example.com",
    "Audience": "example.com",
    "AccessTokenLifeSpanInMinutes": 5,
    "RefreshTokenLifeSpanInDays": 2
  },
  // ... other configurations
}
```

For that, you need to create a class representing the configuration and implementing `ISettings`.
```csharp
using Cayd.AspNetCore.Settings;


public class JwtSettings : ISettings
{
    public static string SettingsKey => "Jwt"; // -> Top-level section name of your configuration

    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessTokenLifeSpanInMinutes { get; set; }
    public int RefreshTokenLifeSpanInDays { get; set; }
}
```

Afterwards, you need to use one of the extension methods of the library to register all configurations you define in an assembly.
```csharp
using Cayd.AspNetCore.Settings.DependencyInjection;


var builder = WebApplication.CreateBuilder();

// via 'IHostApplicationBuilder'
var assembly = Assembly.GetAssembly(typeof(Program));
builder.AddSettingsFromAssembly(assembly);

// or via 'IServiceCollection'
var assembly = Assembly.GetAssembly(typeof(Program));
builder.Services.AddSettingsFromAssembly(builder.Configuration, assembly);
```

You can then use the class you created in your code via the dependency injection system.
```csharp
public class MyService : IMyService
{
    private readonly JwtSettings _jwtSettings;

    public MyService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
}
```

## Extras
- If you want a subsection to be a settings class, you can change the `SettingsKey` value as follows:
```json
{
  "Section": {
    "Subsection1": {
        // ...
    },
    "Subsection2": {
        // ...
    }
  }
}
```
```csharp
public class Subsection1 : ISettings
{
    public static string SettingsKey => "Section:Subsection1";
}

public class Subsection2 : ISettings
{
    public static string SettingsKey => "Section:Subsection2";
}
```

- If you have settings classes in different assemblies, you can use the `AddSettingsFromAssemblies` extension method.
```csharp
var applicationLayerAssembly = Assembly.GetAssembly(typeof(MyClassInApplicationLayer));
var infrastructureLayerAssembly = Assembly.GetAssembly(typeof(MyClassInInfrastructureLayer));
var persistenceLayerAssembly = Assembly.GetAssembly(typeof(MyClassInPersistenceLayer));
var presentationLayerAssembly = Assembly.GetAssembly(typeof(MyClassInPresentationLayer));

// via 'IHostApplicationBuilder'
builder.AddSettingsFromAssemblies(applicationLayerAssembly,
    infrastructureLayerAssembly,
    persistenceLayerAssembly,
    presentationLayerAssembly);

// or via 'IServiceCollection'
builder.Services.AddSettingsFromAssemblies(builder.Configuration,
    applicationLayerAssembly,
    infrastructureLayerAssembly,
    persistenceLayerAssembly,
    presentationLayerAssembly);
```