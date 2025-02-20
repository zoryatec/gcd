using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedLocal;

public class AddLocalDirectoryTests(TestFixture testFixture) : BaseTest(testFixture)
{

     // [Fact]
    private void AddLocalDirectoryTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packagePath = BuildPackage();

       var dir =  new FileInfo(packagePath).Directory ?? throw new NullReferenceException();

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedLocalMenu()
            .WithAddLocalDirectoryCmd()
            .WithPackageLocalDirOpt(dir.ToString())
            .WithFeedLocalDirOpt(feedDir)
            .WithFeedCreateFlag()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        var packageName = Path.GetFileName(packagePath);
        var destinationPackagesContent = File.ReadAllText($"{feedDir}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDir}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDir}\\Packages.stamps");

        destinationPackagesContent.Should().Contain(packageName);
        destinationPackagesStampsContent.Should().Contain(packageName);

    }
    public string BuildPackage()
    {
        // Arrange
        var packageName = "sample-package";
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
}

