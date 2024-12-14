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
        public void ShouldBeAbleToSetAllProperties()
        {
            // Arrange
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";
            var packageHomePage = Guid.NewGuid().ToString();
            var packageMaintainer = Guid.NewGuid().ToString();

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new PackageBuilderInitArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                .WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            packageVersion = "12.34.56.78";

            args = (new PackageBuilderSerVersionArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithVersion(packageVersion)
                .WithMaintainer(packageHomePage)
                .WithHomePage(packageMaintainer)
                .Build();

            // Act
            result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            var content = File.ReadAllText($"{packageBuilderDir}\\control\\control");
            content.Should().Contain(packageVersion);
            content.Should().Contain(packageHomePage);
            content.Should().Contain(packageMaintainer);
        }

        [Fact]
        public void ShouldBeAbleToSetOneProperty()
        {
            // Arrange
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";
            var packageHomePage = Guid.NewGuid().ToString();
            var packageMaintainer = Guid.NewGuid().ToString();

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new PackageBuilderInitArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                .WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            packageVersion = "12.34.56.78";

            args = (new PackageBuilderSerVersionArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithMaintainer(packageMaintainer)
                .Build();

            // Act
            result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            var content = File.ReadAllText($"{packageBuilderDir}\\control\\control");
            content.Should().Contain(packageMaintainer);
        }

    }
}
