using FluentAssertions;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.LabView.LabView;

public class BuildSpecBuildTests(TestFixture testFixture) : BaseTest(testFixture)
{
    [Fact]
    public void BuildSpecTest()
    {
        // Arrange
        var currentDir = Directory.GetCurrentDirectory();
        var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
        var outputDir = _tempDirectoryGenerator.GenerateTempDirectory();
            
        var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithBuildSpecMenu()
            .WithBuildCmd()
            .WithProjectPathOption(sampleProjectPath)
            .WithBuildSpecNameOption("test executable")
            .WithBuildSpecTargetOption("My Computer")
            .WithBuildSpecVersionOption("9.8.7.6")
            .WithBuildSpecOutputDir(outputDir)
            .WithLabViewPathOption(@"C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe")
            .WithLabViewPortOption("3363")
            .Build();
            
        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        File.Exists(@$"{outputDir}\test executable.exe").Should().BeTrue();
        
    }
}