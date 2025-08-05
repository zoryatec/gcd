using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedAzBlob;

public class PullFeedMetaDataTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    public void PullFeedMetaData_ShouldDownloadFiles_WhenFeedIsValid()
    {
        // Arrange
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzurePullTestFeedUri();

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithAzBlobFeedMenu()
            .WithPullMetaDataCmd()
            .WithFeedLocalDirOpt(feedDestinationDirectory)
            .WithAzFeedUriOpt(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();

        File.Exists($"{feedDestinationDirectory}\\Packages").Should().BeTrue();
        File.Exists($"{feedDestinationDirectory}\\Packages.gz").Should().BeTrue();
        File.Exists($"{feedDestinationDirectory}\\Packages.stamps").Should().BeTrue();

        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Be("packages_content");
        destinationPackagesGzContent.Should().Be("packages_gz_content");
        destinationPackagesStampsContent.Should().Be("packages_stamps_content");
    }
}

