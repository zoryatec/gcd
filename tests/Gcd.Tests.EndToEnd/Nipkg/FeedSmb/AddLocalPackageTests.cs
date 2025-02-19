using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedSmb;

public class AddLocalPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    private void AddLocalPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzureAddPkgTestFeedUri();
        // Arrange
        string packagePath = BuildPackage();
        string shareAddress = _config.GetSmbRepoAddress();
        shareAddress = $"{shareAddress}\\add-pkg-nested";
        string username = _config.GetSmbUserName();
        string password = _config.GetSmbPassword();
        
        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedSmbMenu()
            .WithAddLocalPackageCmd()
            .WithPackageLocalPathOpt(packagePath)
            .WithSmbShareAddress(shareAddress)
            .WithSmbUserName(username)
            .WithSmbUserPassword(password)
            .WithFeedCreateFlag()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);


        Pull(shareAddress, feedDir);

        var packageName = Path.GetFileName(packagePath);
        var destinationPackagesContent = File.ReadAllText($"{feedDir}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDir}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDir}\\Packages.stamps");

        destinationPackagesContent.Should().Contain(packageName);
        destinationPackagesStampsContent.Should().Contain(packageName);

    }


    private void Pull(string smbAddress, string feedDirectory)
    {
        // Arrange
        string username = _config.GetSmbUserName();
        string password = _config.GetSmbPassword();

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedSmbMenu()
            .WithPullMetaDataCmd()
            .WithSmbShareAddress(smbAddress)
            .WithSmbUserName(username)
            .WithSmbUserPassword(password)
            .WithFeedLocalDirOpt(feedDirectory)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
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

