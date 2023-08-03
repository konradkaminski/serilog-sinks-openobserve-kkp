using System;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Expressions.Compilation.Linq;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.OpenObserve;

public class LogEntryJsonFormatter : ITextFormatter
{
    readonly JsonValueFormatter _valueFormatter;
    
    public LogEntryJsonFormatter(JsonValueFormatter valueFormatter = null)
    {
        _valueFormatter = valueFormatter ?? new JsonValueFormatter(typeTagName: "$type");
    }
    
    public void Format(LogEvent logEvent, TextWriter output)
    {
        FormatEvent(logEvent, output, _valueFormatter);
        output.WriteLine();
    }

    private static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
    {
        if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
        if (output == null) throw new ArgumentNullException(nameof(output));
        if (valueFormatter == null) throw new ArgumentNullException(nameof(valueFormatter));

        output.Write("{\"@t\":\"");
        output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));
        output.Write("\",\"@m\":");
        var message = logEvent.MessageTemplate.Render(logEvent.Properties, CultureInfo.InvariantCulture);
        JsonValueFormatter.WriteQuotedJsonString(message, output);
        output.Write(",\"@mt\":");
        JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
        output.Write(",\"@i\":\"");
        var id = EventIdHash.Compute(logEvent.MessageTemplate.Text);
        output.Write(id.ToString("x8",CultureInfo.InvariantCulture));
        output.Write('"');
        output.Write(",\"@l\":\"");
        output.Write(logEvent.Level);
        output.Write('\"');

        if (logEvent.Exception != null)
        {
            output.Write(",\"@x\":");
            JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
        }

        foreach (var property in logEvent.Properties)
        {
            var name = property.Key;
            if (name.Length > 0 && name[0] == '@')
            {
                // Escape first '@' by doubling
                name = '@' + name;
            }

            output.Write(',');
            JsonValueFormatter.WriteQuotedJsonString(name, output);
            output.Write(':');
            valueFormatter.Format(property.Value, output);
        }

        output.Write('}');
    }
}