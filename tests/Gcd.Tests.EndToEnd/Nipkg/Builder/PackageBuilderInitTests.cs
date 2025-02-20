using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.Builder
{
    public class PackageBuilderInitTests(TestFixture testFixture) : BaseTest(testFixture)
    { 

        [Fact]
        public void NipkgBuildPackageTest()
        {
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithInitCmd()
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
