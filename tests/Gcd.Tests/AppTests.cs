using FluentAssertions;
using Gcd.CommandHandlers;
using Gcd.Extensions;
using Gcd.LabViewProject;
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
        var args = new[] { "project", "build-spec", "list", "--project-path","sample.lvproj"};
        
        // Act
        int result = app.Execute(args);
        
        // Assert
        result.Should().Be(0);
        console.Out.ToString().Should().Contain("[{\"BuildSpecName\":\"My Packed Library\",\"Type\":\"Packed Library\",\"version\":\"\"},{\"BuildSpecName\":\"sample application\",\"Type\":\"EXE\",\"version\":\"\"},{\"BuildSpecName\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"version\":\"\"}]");
    }
    private CommandLineApplication BuildTestApp(IConsole console)
    {

        var assembly = typeof(Program).Assembly;
        var services = new ServiceCollection()
            .AddSingleton<IVersionizeCommandHandler, VersionizeCommandHandler>()
            .AddScoped<ILabViewProjectProvider, LabViewProjectProvider>()
            .AddSingleton<IConsole>(console)
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
            });
            
        var serviceProvider = services.BuildServiceProvider();
        
        var app = new CommandLineApplication<Program>()
        {
            Name = "gcd",
            Description = "CI/CD tool for G programmers with OCDddd",
        };
            
        app.UseGcdCmd(serviceProvider);
        return app; 
    }
}