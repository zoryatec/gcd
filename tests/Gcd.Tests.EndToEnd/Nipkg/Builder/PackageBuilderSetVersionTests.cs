using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg.Builder
{
    public class PackageBuilderSetVersionTests(TestFixture testFixture) : BaseTest(testFixture)
    {

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

            var args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithInitCmd()
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                //.WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            packageVersion = "12.34.56.78";

            args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithSetVersionCmd()
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

            var args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithInitCmd()
                .WithPackageBuilderDirectory(packageBuilderDir)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                //.WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);


            packageVersion = "12.34.56.78";

            args = new GcdArgBuilder()
                .WithNipkgMenu()
                .WithBuilderMenu()
                .WithSetVersionCmd()
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
