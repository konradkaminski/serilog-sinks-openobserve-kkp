# Serilog Sink for OpenObserve [![Build status](https://ci.appveyor.com/api/projects/status/d73v1w8rejebgtrv/branch/main?svg=true)](https://ci.appveyor.com/project/konradkaminski/serilog-sink-openobserve-kkp/branch/main) ![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.OpenObserve-KKP?logo=nuget)


This Serilog Sink allows to log to [OpenObserve](https://openobserve.ai/).

## What is this sink ?
This project is a sink for the OpenObserver.

## Quick start

Install sink in your project.
```powershell
dotnet add package Serilog.Sinks.OpenObserve-KKP
```

Register the sink in code.
```csharp
var logger = new LoggerConfiguration()
    .WriteTo
    .OpenObserve(
        "url",
        "organization",
        "login",
        "key"
    ).CreateLogger();
```
All required details: `url`, `organization`, `login` and `password` you can find in OpenObserve admin panel (Data sources > Custom).

* You can optionally add parameter `streamName` to write logs to specified stream (default value is `default`).
* You can optionally add parameter `restrictedToMinimumLevel` (default value is `LevelAlias.Minimum`).

Use serilog log method to log details (please check sample project).

```csharp
_logger.Debug("Debug message");
```

### Using `appsettings.json` configuration

First install [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration) package if you don't already have it:

```powershell
dotnet add package Serilog.Settings.Configuration
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
With provided configuration following code should be send:

```json
{"@t":"2023-08-03T20:53:20.2872427Z","@m":"Debug message","@mt":"Debug message","@i":"9515f1e2","@l":"Debug","SourceContext":"OpenObsere.Sample.CustomBackgroundService","Application":"OpenObserve.Tests"}
```
On server side it should looks like:
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

