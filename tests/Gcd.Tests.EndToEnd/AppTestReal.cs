using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class AppTestReal
    {
        IGcdProcess _gcd;
        ITempDirectoryGenerator _tempFolderGenerator;
        public  AppTestReal()
        {
            _gcd = new GcdProcess();
            //_gcd = new GcdProcessApp();
            _tempFolderGenerator = new TempDirectoryGenerator();
        }


        [Fact]
        public void VersionTest()
        {
            // Arrange
            var args = new[] { "--version" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        [Fact]
        public void SystemAddToUserPath()
        {
            // Arrange
            var args = new[] { "system", "add-to-user-path", $"C:\\{Guid.NewGuid().ToString()}" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnCorrectList_WhenValidProjectSpecified()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "list", "--project-path", "sample.lvproj" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            result.Out.ToString().Should().Contain("[{\"Name\":\"My Packed Library\",\"Type\":\"Packed Library\",\"Target\":\"target\",\"Version\":\"version\"},{\"Name\":\"sample application\",\"Type\":\"EXE\",\"Target\":\"target\",\"Version\":\"1.0.0.1\"},{\"Name\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"Target\":\"target\",\"Version\":\"version\"}]");
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnError_WhenValidInvalidProjectSpecified()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "list",
                "--project-path", "invalid.lvproj" 
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().NotBe(0);
            //result.Error.Should().BeEmpty();
        }


        [Fact]
        public void ProjectBuildSpecSetVersion_ShouldExecuteWithoutErrors_WhenValidVersionProvided()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "set-version",
            "--project-path", "sample.lvproj",
            "--build-spec-name", "sample application",
            "--build-spec-type", "sample.lvproj",
            "--build-spec-target", "sample.lvproj",
            "--version", "99.88.77.66"};

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }


        [Fact]
        public void NipkgPullFeedMeta_ShouldNotReturnError_WhenCorrectFeedSpecified()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "list",
                "--project-path", "invalid.lvproj"
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().NotBe(0);
            //result.Error.Should().BeEmpty();
        }

    }
}
