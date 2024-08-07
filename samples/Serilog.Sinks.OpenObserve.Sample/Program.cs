﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Serilog.Sinks.OpenObserve.Sample;

public static class Program
{
    public static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureAppConfiguration((builder) =>
            {
                builder
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<CustomBackgroundService>(true);
            })
            .ConfigureServices((_, services) => { services.AddHostedService<CustomBackgroundService>(); })
            .UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });
}