using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedRclone;

[CollectionDefinition("NonParallelTests", DisableParallelization = true)]
public class NonParallelTestCollection
{
    // Collection class does not need to contain any code.
    // It is just used for organizing the tests.
}

[Collection("NonParallelTests")]
public class AddLocalPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{
    [Fact]
    private void AddLocalPackageTest()
    {

        var feedDir = "GCDSHAREPOINTTEST:/integration-test-feed";
        // Arrange
        string packagePath = BuildPackage();

        
        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedRcloneMenu()
            .WithAddLocalPackageCmd()
            .WithPackageLocalPathOpt(packagePath)
            .WithRcloneFeedDirOption(feedDir)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Assert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }
    public string BuildPackage()
    {
        // Arrange
        var packageName = Guid.NewGuid().ToString();
        var packageVersion = "99.88.77.66";
        var packageInstalationDir = "BootVolume/Zoryatec/sample-package";

        var packageContentDirectory = GetPackageContentDir();
        var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithBuildCmd()
            .WithPackageContentDirectory(packageContentDirectory)
            .WithPackageName(packageName)
            .WithPackageVersion(packageVersion)
            .WithPackageInstalationDir(packageInstalationDir)
            .WithPackageDestinationDir(packageDestinationDirectory)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Assert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        var packagePath = $"{packageDestinationDirectory}\\{packageName}_{packageVersion}_windows_x64.nipkg";
        File.Exists(packagePath).Should().BeTrue();
        return packagePath;
    }
    private string GetPackageContentDir()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
        return packageContentDirectory;
    }
}

