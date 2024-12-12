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
    public class PackageBuilderSetVersionTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PackageBuilderSetVersionTests(TestFixture testFixture)
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
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new PackageBuilderInitArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                .WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();

            packageVersion = "12.34.56.78";

            args = (new PackageBuilderSerVersionArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithVersion(packageVersion)
                .Build();

            // Act
            result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();

            var content = File.ReadAllText($"{packageBuilderDir}\\control\\control");
            content.Should().Contain(packageVersion);

        }

    }
}
