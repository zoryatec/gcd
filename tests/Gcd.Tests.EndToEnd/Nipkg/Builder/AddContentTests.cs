using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.Builder
{
    public class AddContentTests(TestFixture testFixture) : BaseTest(testFixture)
    {
        [Fact]
        public void AddContentTest()
        {
            // Arrange
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";
            var packageContentDirectory = GetPackageContentDir();

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithAddContentCmd()
                .WithPackageBuilderRootDir(packageBuilderDir)
                .WithInatallationTargetRootDir(packageInstalationDir)
                .WithContentSourceDir(packageContentDirectory)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);
            File.Exists($"{packageBuilderDir}\\data\\BootVolume\\Zoryatec\\sample-package\\content-of-test-package.txt");
        }

        private string GetPackageContentDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            return packageContentDirectory;
        }
    }

}
