using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedGit;

public class AddHttpPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact (Skip ="for now")]
    private void AddHttpPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packageUri = "https://github.com/zoryatec/gcd/releases/download/v0.13.0/gcd_0.13.0_windows_x64.nipkg";
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";
        string branchName = "anotherTest";

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedGitCmd()
            .WithAddHttpPackageCmd()
            .WithGitRepoAddressOpt(repoAddress)
            .WithGitBranchNameOpt(branchName)
            .WithGitUserNameOpt(username)
            .WithGitPasswordOpt(password)
            .WithGitCommitterNameOpt(committerName)
            .WithGitCommitterEmailOpt(committerEmail)
            .WithPackageHttpOpt(packageUri)
            .WithFeedCreateFlag()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

    }

    [Fact(Skip = "for now")]
    private void AddHttpPackageTestUseAbs()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packageUri = "https://github.com/zoryatec/gcd/releases/download/v0.13.0/gcd_0.13.0_windows_x64.nipkg";
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";
        string branchName = "anotherTest";

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedGitCmd()
            .WithAddHttpPackageCmd()
            .WithGitRepoAddressOpt(repoAddress)
            .WithGitBranchNameOpt(branchName)
            .WithGitUserNameOpt(username)
            .WithGitPasswordOpt(password)
            .WithGitCommitterNameOpt(committerName)
            .WithGitCommitterEmailOpt(committerEmail)
            .WithPackageHttpOpt(packageUri)
            .WithFeedCreateFlag()
            .WithUseAbsolutePathFlag()
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
}


