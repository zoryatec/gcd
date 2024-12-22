using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg.Feed
{
    public class PullFeedMetaDataTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PullFeedMetaDataTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void PullFeedMetaData_ShouldDownloadFiles_WhenFeedIsValid()
        {
            // Arrange
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzurePullTestFeedUri();

            var args = new PullFeedMetaArgBuilder()
                .WithFeedLocalPath(feedDestinationDirectory)
                .WithFeedUri(feedUri)
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

        [Fact]
        public void PullFeedMetaData_ShoulReturnError_WhenFeedLocalPathNotSpecified()
        {
            // Arrange
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzurePublicFeedUri();

            var args = new PullFeedMetaArgBuilder()
                .WithFeedUri(feedUri)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(1);
            //result.Error.Should().BeEmpty(); // NOT CORRECT SHOUL RETURN ERROR
        }
    }
}
