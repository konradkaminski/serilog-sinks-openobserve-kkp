# Serilog Sink for OpenObserve [![Build status](https://ci.appveyor.com/api/projects/status/d73v1w8rejebgtrv/branch/main?svg=true)](https://ci.appveyor.com/project/konradkaminski/serilog-sink-openobserve-kkp/branch/main)

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
    )
```

More information about using Serilog is available in the [Serilog Documentation](https://github.com/serilog/serilog/wiki).

