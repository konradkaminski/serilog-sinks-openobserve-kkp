using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Serilog.Sinks.OpenObserve.Sample;

public class CustomBackgroundService : IHostedService, IDisposable
{
    private int _executionCount;
    private readonly ILogger<CustomBackgroundService> _logger;
    private readonly Timer _timer;

    public CustomBackgroundService(ILogger<CustomBackgroundService> logger)
    {
        _logger = logger;
        _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(3));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{ServiceName} started, version: {EnvVErsion}.", nameof(CustomBackgroundService),
            Environment.Version.ToString());
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{ServiceName} stopped.", nameof(CustomBackgroundService));
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);

        var objData = new { Counter = count, CounterMessage = $"Counter value is \"{count}\"" };

        _logger.LogInformation(
            "{ServiceName} is working. Count: {Count} with data {Data}", nameof(CustomBackgroundService), count,
            objData);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}