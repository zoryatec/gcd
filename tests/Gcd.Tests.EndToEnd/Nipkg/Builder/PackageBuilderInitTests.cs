using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PackageBuilderInitTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PackageBuilderInitTests(TestFixture testFixture)
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

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new PackageBuilderInitArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                //.WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);
            Directory.Exists($"{packageBuilderDir}\\data\\BootVolume\\Zoryatec\\sample-package");
        }

        private string GetPackageContentDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            return packageContentDirectory;
        }
    }

}
