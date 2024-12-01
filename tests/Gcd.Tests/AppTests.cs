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

public class AppTests
{



    [Fact]
    public void VersionizeTest()
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
    public void VersionTest()
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
    public void SystemAddToUserPath()
    {
        // Arrange
        var console = new FakeConsole();

        var app = BuildTestApp(console);
        var args = new[] { "system", "add-to-user-path","--path", "C:\\sample path" };

        // Act
        int result = app.Execute(args);

        // Assert
        var output = console.Out.ToString();
        var error = console.Out.ToString();
        result.Should().Be(0);
        console.Out.ToString();
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
        console.Out.ToString().Should().Contain("[{\"Name\":\"My Packed Library\",\"Type\":\"Packed Library\",\"Target\":\"target\",\"Version\":\"version\"},{\"Name\":\"sample application\",\"Type\":\"EXE\",\"Target\":\"target\",\"Version\":\"1.0.0.1\"},{\"Name\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"Target\":\"target\",\"Version\":\"version\"}]");
    }
    [Fact]
    public void ProjectSetVersionTest()
    {
        // Arrange
        var console = new FakeConsole();

        var app = BuildTestApp(console);
        var args = new[] { "project", "build-spec", "set-version", 
            "--project-path", "sample.lvproj",
            "--build-spec-name", "sample application",
            "--build-spec-type", "sample.lvproj",
            "--build-spec-target", "sample.lvproj",
            "--version", "99.88.77.66"};


        // Act
        //int result = app.Execute(args);

        // Assert
        var output = console.Out.ToString();
        var error = console.Out.ToString();
        //result.Should().Be(0);
    }

    [Fact]
    public void NipkgTemplateCreate()
    {
        // Arrange
        var console = new FakeConsole();

        var app = BuildTestApp(console);
        var args = new[] { "nipkg", "template", "create",
            "--package-path", "package-template",
            "--package-name", "sample-package",
            "--package-version", "1.0.0.1",
            "--package-destination-dir", "BootVolume/Zoryatec/sample-package"};


        // Act
        int result = app.Execute(args);

        // Assert
        var output = console.Out.ToString();
        var error = console.Out.ToString();

        result.Should().Be(0);
    }

    [Fact]
    public void NipkgPackageCreate()
    {
        // Arrange
        var console = new FakeConsole();

        var app = BuildTestApp(console);
        var args = new[] { "nipkg", "package", "create",
            "--package-sourec-dir", "package-template",
            "--package-name", "sample-package",
            "--package-version", "1.0.0.1",
            "--package-destination-dir", "BootVolume/Zoryatec/sample-package"};


        // Act
        int result = app.Execute(args);

        // Assert
        var output = console.Out.ToString();
        var error = console.Out.ToString();

        result.Should().Be(0);
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