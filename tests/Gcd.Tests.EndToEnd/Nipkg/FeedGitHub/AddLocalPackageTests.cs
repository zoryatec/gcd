using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Services;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedGitHub;

public class AddLocalPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{
    [Fact]
    private async Task AddLocalPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzureAddPkgTestFeedUri();
        // Arrange
        string packagePath = BuildPackage();
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";
        string branchName = "add-package-github-feed";

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedGitHubMenu()
            .WithAddLocalPackageCmd()
            .WithGitRepoAddressOpt(repoAddress)
            .WithGitBranchNameOpt(branchName)
            .WithGitUserNameOpt(username)
            .WithGitPasswordOpt(password)
            .WithGitCommitterNameOpt(committerName)
            .WithGitCommitterEmailOpt(committerEmail)
            .WithPackageLocalPathOpt(packagePath)
            .WithFeedCreateFlag()
            .Build();

        // 
        var ghService = new GitHubReleaseService(password, "zoryatec", "gcd-feed-test");
        await ghService.DeleteAllReleasesAsync();
        await ghService.DeleteAllTagsAsync();
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        Pull(feedDestinationDirectory, branchName); // it wont work for now

        var packageName = Path.GetFileName(packagePath);
        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Contain(packageName);
        destinationPackagesStampsContent.Should().Contain(packageName);

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

    private void Pull(string feedDirectory, string branchName = "push-test")
    {
        // Arrange

        // Arrange
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedGitdMenu()
            .WithPullMetaDataCmd()
            .WithGitRepoAddressOpt(repoAddress)
            .WithGitBranchNameOpt(branchName)
            .WithGitUserNameOpt(username)
            .WithGitPasswordOpt(password)
            .WithGitCommitterNameOpt(committerName)
            .WithGitCommitterEmailOpt(committerEmail)
            .WithFeedLocalDirOpt(feedDirectory)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();
    }
}

