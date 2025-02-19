using FluentAssertions;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Project
{
    public class BuildSpecSetVersionTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        [Fact]
        public void ProjectBuildSpecSetVersion_ShouldExecuteWithoutErrors_WhenValidVersionProvided()
        {
            // Arrange
            var tempDir = _tempDirectoryGenerator.GenerateTempDirectory();
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";


            var tempProjectpath = $"{tempDir}\\sample.lvproj";
            File.Copy(sampleProjectPath, tempProjectpath);

            
            var args = GcdArgBuilder.Create()
                .WithLabViewMenu()
                .WithBuildSpecMenu()
                .WithSetVersionCmd()
                .WithProjectPathOption(tempProjectpath)
                .WithBuildSpecNameOption("sample application")
                .WithBuildSpecTypeOption("invalidForNow")
                .WithBuildSpecTargetOption("invalidForNow")
                .WithBuildSpecVersionOption("99.88.77.66")
                .Build();

            // Act
            var result = _gcd.Run(args);

            var major = @"<Property Name=""Bld_version.major"" Type=""Int"">99</Property>";
            var build = @"<Property Name=""Bld_version.build"" Type=""Int"">66</Property>";

            string content = File.ReadAllText(tempProjectpath);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);
            content.Should().Contain(major);
            content.Should().Contain(build);
            var debugOut = result.Out;
            result.Error.Should().BeEmpty();
        }
    }
}
