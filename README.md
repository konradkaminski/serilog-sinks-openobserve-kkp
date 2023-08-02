# Serilog Sink for OpenObserve [![Build status](https://ci.appveyor.com/api/projects/status/d73v1w8rejebgtrv/branch/main?svg=true)](https://ci.appveyor.com/project/konradkaminski/serilog-sink-openobserve-kkp/branch/main) ![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.OpenObserve-KKP?logo=nuget)


This Serilog Sink allows to log to [OpenObserve](https://openobserve.ai/).

## What is this sink ?
This project is a sink for the OpenObserver.

## Quick start

Install sink in your project.
```powershell
> dotnet add package Serilog.Sinks.OpenObserve
```

Register the sink in code.
```csharp
var logger = new LoggerConfiguration()
    .WriteTo
    .OpenObserve(
        "url",
        "organization"
        "login",
        "password"
    ).CreateLogger();
```

You can optionally add parameter `stream` to write logs to specified stream (default value is `default`)

Use serilog log method to log details.

```csharp
Log.Debug("Debug message");
```

### Using `appsettings.json` configuration

First install package [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) if you don have it:

```powershell
dotnet add package Serilog.Settings.Configuration
```

In your `appsettings.json` file, under the `Serilog` node, add following entries:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.OpenObserve-KKP"],
    "WriteTo": [
      { 
        "Name": "OpenObserve", 
        "Args": { 
          "url": "[api-url]",
          "organization": "[your-organization]",
          "login": "[your-login]",
          "password": "[your-password]"
        }
      }
    ]
  }
}
```

More information about using Serilog is available in the [Serilog Documentation](https://github.com/serilog/serilog/wiki).

