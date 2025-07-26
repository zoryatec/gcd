using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Tools
{
    public class SetIniFileParameterTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        // [Fact]
        public void AddParameterToNonExistentFile()
        {
            // Arrange
            var tempDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var iniFilePath = $"{tempDirectory}\\test.ini";
            var args =  new string[]{"tools", "set-ini-parameter", "--ini-file-path", iniFilePath, "--section", "LabVIEW", "--key", "TestKey", "--value", "TestValue"};
            
            // Act
            var result = _gcd.Run(args);

            // Assert
            Assert.Multiple(() =>
            {
                File.Exists(iniFilePath).Should().BeTrue();
                File.ReadAllLines(iniFilePath).Should().HaveCount(2);
                result.Error.Should().BeEmpty();
                result.Return.Should().Be(0);   
            });
        }
    }
}
