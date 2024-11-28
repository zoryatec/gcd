using FluentAssertions;
using Gcd.CommandHandlers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Gcd.Tests;

public  class AppTests
{
    

    [Fact]
    public  void RunTest()
    {
        // Arrange
        var console = new FakeConsole();
   
        var app = BuildTestApp(console);
        var args = new[] { "config", "list" };
        
        // Act
        int result = app.Execute(args);
        
        // Asssert
        result.Should().Be(0);
        console.Out.ToString().Should().Contain("coreclationTest");
    }
    
    
    [Fact]
    public  void VersionizeTest()
    {
        // Arrange
        var console = new FakeConsole();
   
        var app = BuildTestApp(console);
        var args = new[] { "versionize" };
        
        // Act
        int result = app.Execute(args);
        
        // Asssert
        result.Should().Be(0);
        console.Out.ToString().Should().Contain("versionize!!!");
    }

    private CommandLineApplication BuildTestApp(IConsole console)
    {

        var services = new ServiceCollection()
            .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
            .AddSingleton<IConsole>(console)
            .BuildServiceProvider();
        
        var builder = new GcdAppBuilder();
        var app = builder.Build(services);
        return app; 
    }
}