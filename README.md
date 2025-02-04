# Serilog Sink for OpenObserve [![Build status](https://ci.appveyor.com/api/projects/status/d73v1w8rejebgtrv/branch/main?svg=true)](https://ci.appveyor.com/project/konradkaminski/serilog-sink-openobserve-kkp/branch/main) ![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.OpenObserve-KKP?logo=nuget)


This [Serilog](https://github.com/serilog/serilog) sink allows to log to [OpenObserve](https://openobserve.ai/) as the observability backend from your .NET application.

The sink is built to make use of the REST endpoint of the OpenObserve server.


## Quick start

Precondition: Serilog has to be referenced in your project.

Install sink in your project.
```powershell
dotnet add package Serilog.Sinks.OpenObserve-KKP
```

### Using code configuration
Register the sink in code (Program.cs or similar).
```csharp
var logger = new LoggerConfiguration()
    .WriteTo
    .OpenObserve(
        "url",
        "organization"
        "login",
        "key"
    ).CreateLogger();
```

You can optionally add parameter `streamName` to write logs to specified stream (default value is `default`)

Use serilog log method to log details (please check sample project).

```csharp
_logger.Debug("Debug message");
```

### Using `appsettings.json` configuration

First install [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) package if you don't already have it:

```powershell
dotnet add package Serilog.Settings.Configuration
```

Register the sink in code (Program.cs or similar).
```csharp
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();
```

In your `appsettings.json` file, under the `Serilog` node, add following entries:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.OpenObserve-KKP"],
    "MinimumLevel": "Debug",
    "WriteTo": [
      {
        "Name": "OpenObserve",
        "Args": {
          "url": "https://api.openobserve.ai",
          "organization": "[organization]",
          "login": "[login]",
          "key": "[key]"
        }
      }
    ],
    "Properties": {
      "Application": "OpenObserve.Tests"
    }
  }
}
```

With provided configuration following data will be send to the observability backend:
```json
{"@t":"2023-08-03T20:53:20.2872427Z","@m":"Debug message","@mt":"Debug message","@i":"9515f1e2","@l":"Debug","SourceContext":"OpenObsere.Sample.CustomBackgroundService","Application":"OpenObserve.Tests"}
```

On server side it should look like this:
```json
{
  "_i": "9515f1e2",
  "_l": "Debug",
  "_m": "Debug message",
  "_mt": "Debug message",
  "_t": "2023-08-03T20:53:20.2872427Z",
  "_timestamp": 1691096013274896,
  "application": "OpenObserve.Tests",
  "sourcecontext": "OpenObsere.Sample.CustomBackgroundService"
}
```
Please note:
* field `_timestamp` is added on server side
* field `_mt` contains message template, e.g `Counter: {CounterValue}`
* field `_m` contains rendered message, e.g. `Counter: 2`
* field `_i` is calculated on message template text, it's different for each different message template     

More information about using Serilog is available in the [Serilog Documentation](https://github.com/serilog/serilog/wiki).

