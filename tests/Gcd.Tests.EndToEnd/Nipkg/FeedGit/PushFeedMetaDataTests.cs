using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedGit;

public class PushFeedMetaDataTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    public void PushPull_ShouldMatch()
    {
        // Arrange
        var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

        var sourcePackageContent = Guid.NewGuid().ToString();
        var sourcePackageGzContent = Guid.NewGuid().ToString();
        var sourcePackageStampsContent = Guid.NewGuid().ToString();

        File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);


        Push(feedSourceDirectory);
        Pull(feedDestinationDirectory); // wont work with current implementation of git fs


        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Be(sourcePackageContent);
        destinationPackagesGzContent.Should().Be(sourcePackageGzContent);
        destinationPackagesStampsContent.Should().Be(sourcePackageStampsContent);

        Directory.Delete(feedSourceDirectory, true);
        Directory.Delete(feedDestinationDirectory, true);

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
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }

    private void Push(string feedDirectory, string branchName = "push-test")
    {
        // Arrange
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "test gcd";
        string committerEmail = "mail@mail.com";


        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedGitdMenu()
            .WithPushMetaDataCmd()
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
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

    }

}

