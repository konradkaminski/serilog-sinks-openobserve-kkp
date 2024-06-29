using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.OpenObserve;

public class Sink(HttpClient client) : IBatchedLogEventSink, IDisposable
{
    private readonly LogEntryFormatter _logEntryFormatter = new();

    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        var payload = new StringWriter();
        var logEvents = batch.ToList();
        foreach (var evt in logEvents)
        {
            _logEntryFormatter.Format(evt, payload);
        }

        var result = await client.Send(payload.ToString());
        if (!result.IsSuccess)
        {
            throw new Exception("Incorrect response");
        }
    }

    public Task OnEmptyBatchAsync()
    {
        return EmitBatchAsync([]);
    }

    public void Dispose()
    {
    }
}