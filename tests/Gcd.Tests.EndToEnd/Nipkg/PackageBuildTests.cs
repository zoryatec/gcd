using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PackageBuildTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PackageBuildTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void NipkgBuildPackageTest()
        {
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";

            var packageContentDirectory = GetPackageContentDir();
            var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new PackageBuildArgBuilder())
                .WithPackageContentDirectory(packageContentDirectory)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                .WithPackageInstalationDir(packageInstalationDir)
                .WithPackageDestinationDir(packageDestinationDirectory)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            File.Exists($"{packageDestinationDirectory}\\{packageName}_{packageVersion}_windows_x64.nipkg");
        }

        private string GetPackageContentDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            return packageContentDirectory;
        }
    }

}
