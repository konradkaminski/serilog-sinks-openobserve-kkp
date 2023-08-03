using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.OpenObserve;

public class Sink : IBatchedLogEventSink, IDisposable
{
    private readonly HttpClient _client;
    private readonly LogEntryFormatter _logEntryFormatter;

    public Sink(HttpClient client)
    {
        _logEntryFormatter = new LogEntryFormatter();
        _client = client;
    }
    
    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        var payload = new StringWriter();
        var logEvents = batch.ToList();
        foreach (var evt in logEvents)
        {
            _logEntryFormatter.Format(evt, payload);
        }

        var result = await _client.Send(payload.ToString());
        if (!result.IsSuccess)
        {
            throw new Exception("Incorrect response");
        }
    }

    public Task OnEmptyBatchAsync()
    {
        return EmitBatchAsync(Enumerable.Empty<LogEvent>());
    }

    public void Dispose()
    {
    }
}