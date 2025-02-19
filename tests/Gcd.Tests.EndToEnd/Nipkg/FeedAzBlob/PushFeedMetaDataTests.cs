using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.FeedAzBlob;

public class PushFeedMetaDataTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    public void Push_ShouldReturnEror_WhenPathIsEmpty()
    {
        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithAzBlobFeedMenu()
            .WithPushMetaDataCmd()
            .WithFeedLocalDirOpt("dd")
            .WithAzFeedUriOpt("dd")
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(1);
        result.Error.Should().NotBeEmpty();
    }

    [Fact]
    public void PushPull_ShouldMatch()
    {
        // Arrange
        var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzurePushPullTestFeedUri();

        var sourcePackageContent = Guid.NewGuid().ToString();
        var sourcePackageGzContent = Guid.NewGuid().ToString();
        var sourcePackageStampsContent = Guid.NewGuid().ToString();

        File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
        File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);


        Push(feedSourceDirectory, feedUri);
        Pull(feedDestinationDirectory, feedUri);


        var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

        destinationPackagesContent.Should().Be(sourcePackageContent);
        destinationPackagesGzContent.Should().Be(sourcePackageGzContent);
        destinationPackagesStampsContent.Should().Be(sourcePackageStampsContent);

        Directory.Delete(feedSourceDirectory, true);
        Directory.Delete(feedDestinationDirectory, true);

    }

    private void Pull(string feedDirectory, string feedUri)
    {
        // Arrange

        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithAzBlobFeedMenu()
            .WithPullMetaDataCmd()
            .WithFeedLocalDirOpt(feedDirectory)
            .WithAzFeedUriOpt(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();
    }

    private void Push(string feedDirectory, string feedUri)
    {
        // Arrange
        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithAzBlobFeedMenu()
            .WithPushMetaDataCmd()
            .WithFeedLocalDirOpt(feedDirectory)
            .WithAzFeedUriOpt(feedUri)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

    }

}

