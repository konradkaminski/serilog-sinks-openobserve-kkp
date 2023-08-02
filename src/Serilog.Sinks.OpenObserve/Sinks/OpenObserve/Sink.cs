using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.OpenObserve;

public class Sink : IBatchedLogEventSink, IDisposable
{
    private readonly HttpClient _client;

    public Sink(HttpClient client)
    {
        _client = client;
    }
    
    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {

        var data = batch.Select(x => new LogEntry(x));

        var result = await _client.Send(data);
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