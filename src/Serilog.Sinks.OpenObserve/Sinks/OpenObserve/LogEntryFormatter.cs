using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.OpenObserve;

public class LogEntryFormatter : ITextFormatter
{
    private readonly LogEntryJsonFormatter _jsonFormatter = new(new JsonValueFormatter("$type"));
    
    public void Format(LogEvent logEvent, TextWriter output)
    {
        _jsonFormatter.Format(logEvent, output);
    }
}