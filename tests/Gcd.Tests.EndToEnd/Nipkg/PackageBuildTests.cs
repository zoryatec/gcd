using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PackageBuildTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PackageBuildTests()
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = new TestConfiguration();
        }

        [Fact(Skip = "for now")]
        public void NipkgBuildPackageTest()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = _args.NipkgPackageCreate(
                packageDestinationDirectory: packageDestinationDirectory,
                packageContentDirectory: packageContentDirectory);

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            File.Exists($"{packageDestinationDirectory}\\sample-package_99.88.77.66_windows_x64.nipkg");
        }
    }

}
