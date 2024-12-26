using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg.Feed
{
    public class PushFeedMetaDataTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PushFeedMetaDataTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void Push_ShouldReturnEror_WhenPathIsEmpty()
        {
            var args = new PushAzFeedMetaArgBuilder()
                .WithFeedLocalPath("dd")
                .WithFeedUri("dd")
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

            var args = new PullFeedMetaArgBuilder()
                .WithFeedLocalPath(feedDirectory)
                .WithFeedUri(feedUri)
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
            var args = new PushAzFeedMetaArgBuilder()
                .WithFeedLocalPath(feedDirectory)
                .WithFeedUri(feedUri)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);

        }

    }
}
