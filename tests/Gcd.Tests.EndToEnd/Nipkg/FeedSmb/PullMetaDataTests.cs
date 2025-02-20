using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedSmb;

public class PullFeedMetaDataTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    public void PullFeedMetaData_ShouldDownloadFiles_WhenFeedIsValid()
    {
        // Arrange
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string shareAddress = _config.GetSmbRepoAddress();
        string username = _config.GetSmbUserName();
        string password = _config.GetSmbPassword();
        shareAddress = $"{shareAddress}\\pull";

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithFeedSmbMenu()
            .WithPullMetaDataCmd()
            .WithSmbShareAddress(shareAddress)
            .WithSmbUserName(username)
            .WithSmbUserPassword(password)
            .WithFeedLocalDirOpt(feedDestinationDirectory)
            .Build();


        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);


        File.Exists($"{feedDestinationDirectory}\\Packages").Should().BeTrue();
        File.Exists($"{feedDestinationDirectory}\\Packages.gz").Should().BeTrue();
        File.Exists($"{feedDestinationDirectory}\\Packages.stamps").Should().BeTrue();

        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Contain("packages_content");
        destinationPackagesGzContent.Should().Be("packages_gz_content"); // it's binary, need to pull it to local dir and update
        destinationPackagesStampsContent.Should().Contain("packages_stamps_content");
    }

}

