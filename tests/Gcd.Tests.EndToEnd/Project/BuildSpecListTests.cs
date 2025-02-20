using FluentAssertions;
using Gcd.Tests.EndToEnd.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Project
{
    public class BuildSpecListTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        // [Fact]
        public void ProjectBuildSpecList_ShouldReturnCorrectList_WhenValidProjectSpecified()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
            // var args = new[] { "project", "build-spec", "list", "--project-path", $"{sampleProjectPath}" };

            
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = GcdArgBuilder.Create()
                .WithLabViewMenu()
                .WithBuildSpecMenu()
                .WithListCmd()
                .WithProjectPathOption(sampleProjectPath)
                .Build();
            
            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);
            result.Out.ToString().Should().Contain("[{\"Name\":\"My Packed Library\",\"Type\":\"Packed Library\",\"Target\":\"target\",\"Version\":\"version\"},{\"Name\":\"sample application\",\"Type\":\"EXE\",\"Target\":\"My Computer\",\"Version\":\"1.0.0.1\"},{\"Name\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"Target\":\"target\",\"Version\":\"version\"}]");
        }

        // [Fact]
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
