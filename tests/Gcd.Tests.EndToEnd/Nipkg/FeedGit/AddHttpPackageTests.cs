using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.Tests.EndToEnd.Setup.Arguments;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedGit;

public class AddHttpPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{

   // [Fact]
    private void AddHttpPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packageUri = "https://github.com/zoryatec/gcd/releases/download/0.23.13/gcd_0.23.13_windows_x64.nipkg";
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";
        string branchName = "add-http-package-nested";

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedGitdMenu()
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

    //[Fact]
    private void AddHttpPackageTestUseAbs()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packageUri = "https://github.com/zoryatec/gcd/releases/download/0.23.13/gcd_0.23.13_windows_x64.nipkg";
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";
        string branchName = "add-http-package-relative";

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedGitdMenu()
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


