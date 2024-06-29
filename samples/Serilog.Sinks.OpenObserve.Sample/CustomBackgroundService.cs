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
        _logger.LogInformation("{ServiceName} started.", nameof(CustomBackgroundService));
        Console.WriteLine($"{nameof(CustomBackgroundService)} started.");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{ServiceName} stopped.", nameof(CustomBackgroundService));
        Console.WriteLine($"{nameof(CustomBackgroundService)} stopped.");
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);
        _logger.LogInformation(
            "{ServiceName} is working. Count: {Count}", nameof(CustomBackgroundService), count);
        Console.WriteLine($"{nameof(CustomBackgroundService)} is working. Count: {count}");
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}