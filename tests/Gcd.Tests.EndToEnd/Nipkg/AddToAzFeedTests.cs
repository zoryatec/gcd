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
    public class AddToAzFeedTests : IClassFixture<TestFixture>
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public AddToAzFeedTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

        [Fact]
        public void AddPackageToAZ()
        {
            // Arrange
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzureAddPkgTestFeedUri();

            // Act
            ClearFeed(feedUri);

            var packagePath = BuildPackage();

            AddPackage(packagePath, feedUri);

            string packageName =  Path.GetFileName(packagePath);


            Pull(feedDestinationDirectory, feedUri);


            var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
            var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
            var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");
            //File.Exists($"{feedDestinationDirectory}\\{packageName}").Should().BeTrue();

            //destinationPackagesContent.Should().Be(sourcePackageContent);
            //destinationPackagesGzContent.Should().Be(sourcePackageGzContent);
            //destinationPackagesStampsContent.Should().Be(sourcePackageStampsContent);

            Directory.Delete(feedDestinationDirectory, true);

        }

        public void ClearFeed(string feedUri)
        {
            var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();


            var sourcePackageContent = "";
            var sourcePackageGzContent = "";
            var sourcePackageStampsContent = "";

            File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);
            Push(feedSourceDirectory, feedUri);
        }

        public string BuildPackage()
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
            var packagePath = $"{packageDestinationDirectory}\\{packageName}_{packageVersion}_windows_x64.nipkg";
            File.Exists(packagePath);
            return packagePath;
        }

        private string GetPackageContentDir()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            return packageContentDirectory;
        }




        private void Pull(string feedDirectory, string feedUri)
        {
            // Arrange
            var args = new[] {
                "nipkg", "pull-feed-meta",
                "--feed-local-path", $"{feedDirectory}",
                "--feed-uri", $"{feedUri}"
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        private void Push(string feedDirectory, string feedUri)
        {
            // Arrange
            var args = new[] {
                "nipkg", "push-feed-meta",
                "--feed-local-path", $"{feedDirectory}",
                "--feed-uri", $"{feedUri}"
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        private void AddPackage(string packagePath, string feedUri)
        {
            // Arrange
            var args = new[] {
                "nipkg", "add-package-blob-feed",
                "--package-path", $"{packagePath}",
                "--feed-url", $"{feedUri}"
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

    }
}
