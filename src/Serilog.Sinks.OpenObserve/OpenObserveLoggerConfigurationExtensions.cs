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

        if (string.IsNullOrEmpty(url)) throw new ArgumentException(null, nameof(url));
        if (string.IsNullOrEmpty(organization)) throw new ArgumentException(null, nameof(organization));
        if (string.IsNullOrEmpty(login)) throw new ArgumentException(null, nameof(login));
        if (string.IsNullOrEmpty(key)) throw new ArgumentException(null, nameof(key));
        var sink = new Sink(new HttpClient(url, organization, login, key, streamName));
        return loggerConfiguration.Sink(new PeriodicBatchingSink(sink, new PeriodicBatchingSinkOptions()));
    }
}