using System;
using System.Globalization;
using System.IO;
using Serilog.Events;
using Serilog.Expressions.Compilation.Linq;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.OpenObserve;

public class LogEntryJsonFormatter(JsonValueFormatter valueFormatter = null) : ITextFormatter
{
    private const string TypeTagName = "$type";

    private readonly JsonValueFormatter _valueFormatter =
        valueFormatter ?? new JsonValueFormatter(typeTagName: TypeTagName);

    public void Format(LogEvent logEvent, TextWriter output)
    {
        if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
        if (output == null) throw new ArgumentNullException(nameof(output));

        FormatEvent(logEvent, output, _valueFormatter);
        output.WriteLine();
    }

    private static void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
    {
        var message = logEvent.MessageTemplate.Render(logEvent.Properties, CultureInfo.InvariantCulture);
        var id = EventIdHash.Compute(logEvent.MessageTemplate.Text);

        output.Write($"{{\"@t\":\"{logEvent.Timestamp.UtcDateTime:O}\",\"@m\":");
        JsonValueFormatter.WriteQuotedJsonString(message, output);
        output.Write($",\"@mt\":");
        JsonValueFormatter.WriteQuotedJsonString(logEvent.MessageTemplate.Text, output);
        output.Write($",\"@i\":\"{id:x8}\",\"@l\":\"{logEvent.Level}\"");

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