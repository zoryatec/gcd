using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Project
{
    public class BuildSpecListTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public BuildSpecListTests()
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = new TestConfiguration();
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnCorrectList_WhenValidProjectSpecified()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
            var args = new[] { "project", "build-spec", "list", "--project-path", $"{sampleProjectPath}" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            //result.Out.ToString().Should().Contain("[{\"Name\":\"My Packed Library\",\"Type\":\"Packed Library\",\"Target\":\"target\",\"Version\":\"version\"},{\"Name\":\"sample application\",\"Type\":\"EXE\",\"Target\":\"target\",\"Version\":\"1.0.0.1\"},{\"Name\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"Target\":\"target\",\"Version\":\"version\"}]");
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnError_WhenValidInvalidProjectSpecified()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "list",
                "--project-path", "invalid.lvproj"
                };

            // Act
            //var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().NotBe(0);
            //result.Error.Should().BeEmpty();
        }
    }
}
