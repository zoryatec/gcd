﻿using Gcd.Tests.EndToEnd;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime;

public class TestFixture
{
    public IServiceProvider ServiceProvider { get; }

    public TestFixture()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Path to your test project
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Setup DI

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IConfiguration>(x => configuration);
        serviceCollection.AddScoped<TestConfiguration>();

        // Register settings
        //object value = serviceCollection.Configure<MySettings>(configuration.GetSection(nameof(MySettings)));

        // Register other dependencies (if needed)
        // serviceCollection.AddTransient<IMyService, MyService>();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
}

public class MySettings
{
}