using FluentAssertions;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.LabView.LabView;

public class ProjectBuildTests(TestFixture testFixture) : BaseTest(testFixture)
{
    [Fact]
    public void ProjectBuildTest()
    {
        // Arrange
        var currentDir = Directory.GetCurrentDirectory();
        var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
        var outputDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var version = "9.8.7.6";
        var versionPkg = "9.8.7-6";

        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithBuildProjectCmd()
            .WithProjectPathOption(sampleProjectPath)
            .WithProjectVersionOption(version)
            .WithProjectOutputDirOption(outputDir)
            .WithLabViewPathOption(@"C:\Program Files (x86)\National Instruments\LabVIEW 2023\LabVIEW.exe")
            .WithLabViewPortOption("3363")
            .Build();
            
        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        File.Exists(@$"{outputDir}\My Computer\test executable\test executable.exe").Should().BeTrue();
        File.Exists(@$"{outputDir}\My Computer\test executable 2\test executable 2.exe").Should().BeTrue();
        File.Exists(@$"{outputDir}\My Computer\test package\package\test-packages_{versionPkg}_windows_all.nipkg").Should().BeTrue();
        File.Exists(@$"{outputDir}\My Computer\test package\installer\Install.exe").Should().BeTrue();

    }
}