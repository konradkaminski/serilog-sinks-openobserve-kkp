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
        "organization",
        "login",
        "key"
    ).CreateLogger();
```

You can optionally add parameter `streamName` to write logs to specified stream (default value is `default`)

Use serilog log method to log details (please check sample project).

```csharp
logger.Debug("Debug message");
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

In your `appsettings.json` file, extend the `Serilog` node or fully add the following:

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.OpenObserve-KKP"], // Enable OpenObserve-KKP sink
    "MinimumLevel": "Debug", // General minimum level of Serilog
    "WriteTo": [
      {
        "Name": "OpenObserve", // Name of this sink, don't change
        "Args": {
          "url": "https://api.openobserve.ai",
          "organization": "[organization]",
          "login": "[username]",
          "key": "[password or token]",
          "streamName": "[custom stream name if desired]", // Optional
          "minimumLevel": "Information" // Optional
        }
      }
    ],
    "Properties": {
      "Application": "OpenObserve.Tests"
    }
  }
}
```

Use serilog log method to log details (please check sample project).

```csharp
logger.Debug("Debug message");
```

### Data generated

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

## Configuration specification
### Parameters
| Parameter | Description | Example |
| -- | -- | -- |
| **Url** | OpenObserve api endpoint | `https://api.openobserve.ai` (SaaS) <br/>`https://myserver.com/api` (self-hosted) |
| **Organization** | Identifier of your organization within OpenObserve. Can be seen in the web interface on the top right. <br/> OpenObserve SaaS: please ensure to use _your_ organization identifier. <br/>Self-hosted: you may use any value you prefer. Defaults to "default". The organization will be created automatically. | `SoftwareBros` |
| **Login** | Username aka mail address of the OpenObserve user | `name@myserver.com` |
| **Key** | Password or Token for authentication for the provided user | `SecureToken` |
| **StreamName** | ObenObserve stream identifier | `default` |
| **MinimumLevel** | Overwrite minimum log severity to be handled for this sink | `Information`<br/>Default: the same as Serilog global </br>Options: `Serilog.Events.LogEventLevel`  |

Please note:
It is NOT recommended to store your user password in a configuration of your application. Those credentials allow to login into the OpenObserve web interface. It is highly recommended to _only_ use the token as key.

### What is a token?
A token is a generated text string, having the function of a password, usually used for authentication. It allows to authenticate against the api without providing your actual user password.

### How to retrieve my credentials / token
The OpenObserve web interface provides configuration examples under "Data sources". 

"Data sources" - "Custom" - "Logs" - "Curl" shows the current username and token (format: `username:token`).  
"Data sources" - "Custom" - "Logs" - "Syslog-Ng" shows a configuration similiar (not identical!) to one required for this sink.  

### What is an organization?
An organization is a management unit of users. For the self hosted system you may create any amount of organizations. The active organization can be seen in the web interface top right corner.

### What is a stream?
A stream is a storage endpoint within OpenObserve. Streams can be created in the web interface but will also be automatically created as soon as log data specifying a stream is inbound. A stream contains specific storage configuration such as data retention.

### REST
The sink is creating a REST POST command to the provided endpoint. Specifically the data will be sent to endpoint `POST /api/{organization}/{stream}/_multi`.

Please refer to the [OpenObserve API documentation](https://openobserve.ai/docs/api/) for more information.