using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.OpenObserve;

public class OpenObserveSink : IBatchedLogEventSink, IDisposable
{
    private readonly OpenObserveHttpClient _client;

    public OpenObserveSink(OpenObserveHttpClient client)
    {
        _client = client;
    }
    
    public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
    {
        var result = await _client.Send(batch);
        if (!result.IsSuccess)
        {
            throw new Exception(result.Message);
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