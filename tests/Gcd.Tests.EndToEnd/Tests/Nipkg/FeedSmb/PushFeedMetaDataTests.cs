using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup;


namespace Gcd.Tests.EndToEnd.Nipkg.FeedSmb;

public class PushFeedMetaDataTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact(Skip = "wont work for now")]
    //[Fact]
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

    private void Pull(string feedDirectory)
    {
        // Arrange
        string shareAddress = _config.GetSmbRepoAddress();
        string username = _config.GetSmbUserName();
        string password = _config.GetSmbPassword();

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedSmbCmd()
            .WithPullMetaData()
            .WithSmbShareAddress(shareAddress)
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

    private void Push(string feedDirectory)
    {
        // Arrange
        string shareAddress = _config.GetSmbRepoAddress();
        string username = _config.GetSmbUserName();
        string password = _config.GetSmbPassword();

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedSmbCmd()
            .WithPushMetaData()
            .WithSmbShareAddress(shareAddress)
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

}

