using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg.Feed;

public class AddToAzFeedTests : IClassFixture<TestFixture>
{
    IGcdProcess _gcd;
    GcdArgsBuilder _args;
    ITempDirectoryGenerator _tempDirectoryGenerator;
    TestConfiguration _config;
    public AddToAzFeedTests(TestFixture testFixture)
    {
        _gcd = new GcdProcessApp();
        _args = new GcdArgsBuilder();
        _tempDirectoryGenerator = new TempDirectoryGenerator();
        _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
    }

    [Fact]
    public void AddPackageToAZ()
    {
        // Arrange
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzureAddPkgTestFeedUri();

        ClearFeed(feedUri);

        var packagePath = BuildPackage();
        var packageName = Path.GetFileName(packagePath);

        // Act
        AddPackage(packagePath, feedUri);


        // Assert
        PullMetaData(feedDestinationDirectory, feedUri);

        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Contain(packageName);
        destinationPackagesStampsContent.Should().Contain(packageName);

        Directory.Delete(feedDestinationDirectory, true);
    }

    private void ClearFeed(string feedUri)
    {
        var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();


        var sourcePackageContent = "";
        var sourcePackageGzContent = "";
        var sourcePackageStampsContent = "";

        File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);
        PushMetadata(feedSourceDirectory, feedUri);
    }

    public string BuildPackage()
    {
        // Arrange
        var packageName = "sample-package";
        var packageVersion = "99.88.77.66";
        var packageInstalationDir = "BootVolume/Zoryatec/sample-package";

        var packageContentDirectory = GetPackageContentDir();
        var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

        var args = new PackageBuildArgBuilder()
            .WithPackageContentDirectory(packageContentDirectory)
            .WithPackageName(packageName)
            .WithPackageVersion(packageVersion)
            .WithPackageInstalationDir(packageInstalationDir)
            .WithPackageDestinationDir(packageDestinationDirectory)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
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

    private void PullMetaData(string feedDirectory, string feedUri)
    {
        // Arrange
        var args = new PullFeedMetaArgBuilder()
            .WithFeedLocalPath(feedDirectory)
            .WithFeedUri(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }

    private void PushMetadata(string feedDirectory, string feedUri)
    {
        // Arrange
        var args = new PushAzFeedMetaArgBuilder()
            .WithFeedLocalPath(feedDirectory)
            .WithFeedUri(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }

    private void AddPackage(string packagePath, string feedUri)
    {
        // Arrange

        var args = new AddToAzFeedArgBuilder()
            .WithPackagePath(packagePath)
            .WithAzFeedUri(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }
}

