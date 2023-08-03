using Serilog.Configuration;
using Serilog.Sinks.OpenObserve;
using Serilog.Sinks.PeriodicBatching;
using System;

namespace Serilog;

public static class OpenObserveLoggerConfigurationExtensions
{
    public static LoggerConfiguration OpenObserve(
        this LoggerSinkConfiguration loggerConfiguration,
        string url,
        string organization,
        string login = "", 
        string key = "",
        string streamName = "default"
        )
    {

        if (string.IsNullOrEmpty(url)) throw new ArgumentException(nameof(url));
        if (string.IsNullOrEmpty(organization)) throw new ArgumentException(nameof(organization));
        if (string.IsNullOrEmpty(login)) throw new ArgumentException(nameof(login));
        if (string.IsNullOrEmpty(key)) throw new ArgumentException(nameof(key));
        var sink = new Sink(new HttpClient(url, organization, login, key, streamName));
        return loggerConfiguration.Sink(new PeriodicBatchingSink(sink, new PeriodicBatchingSinkOptions()));
    }
}