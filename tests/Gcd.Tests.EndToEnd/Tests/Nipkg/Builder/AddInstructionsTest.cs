using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class AddInstructionsTest : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public AddInstructionsTest(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void AddContentTest()
        {
            // Arrange
            var packageContentDirectory = GetPackageContentDir();

            var packageBuilderDir = _tempDirectoryGenerator.GenerateTempDirectory();

            //        <customExecute root=""BootVolume"" step=""install"" schedule=""post"" exeName=""Program Files\gcd\gcd.exe"" arguments=""tools add-to-user-path C:\PROGRA~1\gcd"" />
            InitialiseDir(packageBuilderDir);

            var root = "BootVolume";
            var arguments = "tools add-to-user-path C:\\PROGRA~1\\gcd";
            var exeName = "Program Files\\gcd\\gcd.exe";
            var step = "install";
            var schedule = "post";


            var args = (new AddInstructionArgBuilder())
                .WithPackageBuilderRootDir(packageBuilderDir)
                .WithRoot(root)
                .WithArugments(arguments)
                .WithExeName(exeName)
                .WithStep(step)
                .WithSchedule(schedule)
                .Build();

            // Act
            _gcd = new GcdProcessApp(); 
            var result = _gcd.Run(args);

            // Asssert
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);

            File.Exists($"{packageBuilderDir}\\data\\instructions");
            var content = File.ReadAllText($"{packageBuilderDir}\\data\\instructions");
        }

        private string GetPackageContentDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            return packageContentDirectory;
        }


        private void InitialiseDir(string packageBuilderPath)
        {
            var packageName = "sample-package";
            var packageVersion = "99.88.77.66";
            //var packageInstalationDir = "BootVolume/Zoryatec/sample-package";
            var packageHomePage = Guid.NewGuid().ToString();
            var packageMaintainer = Guid.NewGuid().ToString();

            //var packageContentDirectory = GetPackageContentDir();


            var args = (new PackageBuilderInitArgBuilder())
                .WithPackageBuilderDirectory(packageBuilderPath)
                .WithPackageName(packageName)
                .WithPackageVersion(packageVersion)
                //.WithPackageBuilderInstalationDir(packageInstalationDir)
                .Build();

            var result = _gcd.Run(args);
            result.Error.Should().BeEmpty();
            result.Return.Should().Be(0);
        }
    }

}
