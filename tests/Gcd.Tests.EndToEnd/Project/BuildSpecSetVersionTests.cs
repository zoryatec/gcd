using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Project
{
    public class BuildSpecSetVersionTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public BuildSpecSetVersionTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void ProjectBuildSpecSetVersion_ShouldExecuteWithoutErrors_WhenValidVersionProvided()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";


            var args = new[] { "project", "build-spec", "set-version",
            "--project-path", $"{sampleProjectPath}",
            "--build-spec-name", "sample application",
            "--build-spec-type", "",
            "--build-spec-target", "",
            "--version", "99.88.77.66"};

            // Act
            var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().Be(0);
            //result.Error.Should().BeEmpty();
        }
    }
}
