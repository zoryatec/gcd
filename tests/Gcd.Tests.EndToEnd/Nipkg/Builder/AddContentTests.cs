using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class AddContentTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public AddContentTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void AddContentTest()
        {
            // Arrange
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            var packageInstalationDir = "BootVolume/Zoryatec/sample-package";
            var packageContentDirectory = GetPackageContentDir();

            //var packageContentDirectory = GetPackageContentDir();
            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = (new AddContentArgBuilder())
                .WithPackageBuilderRootDir(packageBuilderDir)
                .WithInatallationTargetRootDir(packageInstalationDir)
                .WithContentSourceDir(packageContentDirectory)
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
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
