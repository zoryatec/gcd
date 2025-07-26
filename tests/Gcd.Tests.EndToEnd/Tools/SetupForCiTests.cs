using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Tools
{
    public class SetupForCiTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        [Fact]
        public void SetupHappyCase()
        {
            // Arrange
            var tempDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var labviewIniFilePath = $"{tempDirectory}\\LabVIEW.ini";
            var labviewCliIniFilePath = $"{tempDirectory}\\LabVIEWCli.ini";
            var args =  new string[]{"tools", "setup-system-for-ci", "--labview-cli-ini-file-path", labviewCliIniFilePath, "--labview-ini-file-path", labviewIniFilePath};
            
            // Act
            var result = _gcd.Run(args);

            // Assert
            Assert.Multiple(() =>
            {
                File.Exists(labviewIniFilePath).Should().BeTrue();
                File.Exists(labviewCliIniFilePath).Should().BeTrue();
                File.ReadAllLines(labviewIniFilePath).Should().HaveCount(6);
                File.ReadAllLines(labviewCliIniFilePath).Should().HaveCount(2);
                result.Error.Should().BeEmpty();
                result.Return.Should().Be(0);   
            });
        }
    }
}
