using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Sinks.OpenObserve;

public record LogEntry(
    DateTimeOffset TimeStamp, 
    LogEventLevel LogLevel, 
    string LevelName, 
    string MessageTemplate, 
    string Message, 
    Dictionary<string, string> Data, 
    string ExceptionMessage, 
    string ExceptionStackTrace)
{
    public LogEntry(LogEvent @event) : this(
        @event.Timestamp,
        @event.Level, 
        GetLogLevelName(@event.Level),
        @event.MessageTemplate?.Text,
        @event.MessageTemplate?.Render(@event.Properties),
        GetProperties(@event.Properties),
        @event.Exception?.Message,
        @event.Exception?.StackTrace
        )
    {
    }

    private const string LOG_LEVEL_VERBOSE = "Verbose";
    private const string LOG_LEVEL_DEBUG = "Debug";
    private const string LOG_LEVEL_INFORMATION = "Information";
    private const string LOG_LEVEL_WARNING = "Warning";
    private const string LOG_LEVEL_ERROR = "Error";
    private const string LOG_LEVEL_FATAL = "Fatal";

    private static string GetLogLevelName(LogEventLevel level) => level switch
    {
        LogEventLevel.Verbose => LOG_LEVEL_VERBOSE,
        LogEventLevel.Debug => LOG_LEVEL_DEBUG,
        LogEventLevel.Information => LOG_LEVEL_INFORMATION,
        LogEventLevel.Warning => LOG_LEVEL_WARNING,
        LogEventLevel.Error => LOG_LEVEL_ERROR,
        _ => LOG_LEVEL_FATAL,
    };

    private static Dictionary<string, string> GetProperties(IReadOnlyDictionary<string, LogEventPropertyValue> properties) => properties.ToDictionary(x => x.Key, x => x.Value.ToString());
}
