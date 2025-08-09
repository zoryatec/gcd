using FluentAssertions;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.LabView.LabView;

public class RunViTests(TestFixture testFixture) : BaseTest(testFixture)
{
    // [Fact]
    public void RunViTest()
    {
        // Arrange
        var currentDir = Directory.GetCurrentDirectory();
        var testViPath = $"{currentDir}\\testdata\\labview\\run-vi-test.vi";
        var correlationId = Guid.NewGuid().ToString();

        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithRunViCmd()
            .WithViArgument(correlationId)
            .WithViPathOption(testViPath)
            .WithLabViewPathOption(@"C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe")
            .WithLabViewPortOption("3363")
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Out.Should().Contain(correlationId);
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }
    
    [Fact]
    public void ShouldExecuteWithoutAguments()
    {
        // Arrange
        var currentDir = Directory.GetCurrentDirectory();
        var testViPath = $"{currentDir}\\testdata\\labview\\run-vi-test.vi";
        var correlationId = Guid.NewGuid().ToString();

        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithRunViCmd()
            .WithViPathOption(testViPath)
            .WithLabViewPathOption(@"C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe")
            .WithLabViewPortOption("3363")
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }
    
    [Fact]
    public void ShouldOutputErrorIfFailed()
    {
        // Arrange
        var currentDir = Directory.GetCurrentDirectory();
        var testViPath = $"{currentDir}\\testdata\\labview\\run-vi-test-fail.vi";
        var correlationId = Guid.NewGuid().ToString();

        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithRunViCmd()
            .WithViArgument(correlationId)
            .WithViPathOption(testViPath)
            .WithLabViewPathOption(@"C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe")
            .WithLabViewPortOption("3363")
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().Contain(correlationId);
        result.Return.Should().NotBe(0);
    }
}