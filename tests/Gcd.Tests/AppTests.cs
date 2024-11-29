using FluentAssertions;
using Gcd.CommandHandlers;
using Gcd.Extensions;
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
    
    [Fact]
    public  void VersionTest()
    {
        // Arrange
        var console = new FakeConsole();
   
        var app = BuildTestApp(console);
        var args = new[] { "--version" };
        
        // Act
        int result = app.Execute(args);
        
        // Asssert
        result.Should().Be(0);
        // does not work in test
        var output = console.Out.ToString();
    }
    
    [Fact]
    public  void ProjectTest()
    {
        // Arrange
        var console = new FakeConsole();
   
        var app = BuildTestApp(console);
        var args = new[] { "project", "build-spec", "list", "--project-path","dddd"};
        
        // Act
        int result = app.Execute(args);
        
        // Assert
        result.Should().Be(0);
        console.Out.ToString().Should().Contain("[{\"BuildSpecName\":\"testName\",\"Target\":\"testTarget\",\"version\":\"1.0.0\"}]");
    }
    private CommandLineApplication BuildTestApp(IConsole console)
    {

        var services = new ServiceCollection()
            .AddSingleton<IProjectService, ProjectService>()
            .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
            .AddSingleton<IConsole>(console)
            .BuildServiceProvider();
        
        var app = new CommandLineApplication<Program>()
        {
            Name = "gcd",
            Description = "CI/CD tool for G programmers with OCDddd",
        };
            
        app.UseGcdCmd(services);
        return app; 
    }
}