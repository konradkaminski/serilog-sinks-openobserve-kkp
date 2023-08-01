# Serilog Sink for OpenObserve
This Serilog Sink allows to log to [OpenObserve](https://openobserve.ai/).

## What is this sink ?
This project is a sink for the OpenObserver.

## Quick start

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

