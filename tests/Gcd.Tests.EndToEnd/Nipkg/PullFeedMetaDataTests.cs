using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PullFeedMetaDataTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PullFeedMetaDataTests()
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = new TestConfiguration();
        }

        [Fact]
        public void PullFeedMetaData_ShouldDownloadFiles_WhenFeedIsValid()
        {
            // Arrange
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzurePublicFeedUri();

            var args = (new PullFeedMetaArgBuilder())
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
        }

        [Fact]
        public void PullFeedMetaData_ShoulReturnError_WhenFeedLocalPathNotSpecified()
        {
            // Arrange
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzurePublicFeedUri();

            var args = (new PullFeedMetaArgBuilder())
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
