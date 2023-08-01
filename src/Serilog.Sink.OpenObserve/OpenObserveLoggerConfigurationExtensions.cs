using Serilog.Configuration;
using Serilog.Sinks.OpenObserve;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog;

public static class OpenObserveLoggerConfigurationExtensions
{
    public static LoggerConfiguration OpenObserve(
        this LoggerSinkConfiguration loggerConfiguration,
        string url,
        string organization,
        string login, 
        string password,
        string streamName = "default"
        )
    {
        var sink = new OpenObserveSink(new OpenObserveHttpClient(url, organization, login, password, streamName));
        var options = new PeriodicBatchingSinkOptions();
        return loggerConfiguration.Sink(new PeriodicBatchingSink(sink, options));
    }
}