using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var args = new[] { "project", "build-spec", "set-version",
            "--project-path", $"{tempProjectpath}",
            "--build-spec-name", "sample application",
            "--build-spec-type", "invalidForNow",
            "--build-spec-target", "invalidForNow",
            "--version", "99.88.77.66"};

            // Act
            var result = _gcd.Run(args);

            var major = @"<Property Name=""Bld_version.major"" Type=""Int"">99</Property>";
            var build = @"<Property Name=""Bld_version.build"" Type=""Int"">66</Property>";

            string content = File.ReadAllText(tempProjectpath);

            // Asssert
            result.Return.Should().Be(0);
            content.Should().Contain(major);
            content.Should().Contain(build);
            var debugOut = result.Out;
            result.Error.Should().BeEmpty();
        }
    }
}
