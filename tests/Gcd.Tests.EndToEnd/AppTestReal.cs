﻿using FluentAssertions;
using Gcd.LabViewProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class AppTestReal
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        public  AppTestReal()
        {
            //_gcd = new GcdProcess();
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
        }


        [Fact]
        public void VersionTest()
        {
            // Arrange
            var args = new[] { "--version" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        [Fact]
        public void SystemAddToUserPath()
        {
            // Arrange
            var args = new[] { "system", "add-to-user-path", $"C:\\{Guid.NewGuid().ToString()}" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnCorrectList_WhenValidProjectSpecified()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
            var args = new[] { "project", "build-spec", "list", "--project-path", $"{sampleProjectPath}" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            //result.Out.ToString().Should().Contain("[{\"Name\":\"My Packed Library\",\"Type\":\"Packed Library\",\"Target\":\"target\",\"Version\":\"version\"},{\"Name\":\"sample application\",\"Type\":\"EXE\",\"Target\":\"target\",\"Version\":\"1.0.0.1\"},{\"Name\":\"Sample Package\",\"Type\":\"{E661DAE2-7517-431F-AC41-30807A3BDA38}\",\"Target\":\"target\",\"Version\":\"version\"}]");
        }

        [Fact]
        public void ProjectBuildSpecList_ShouldReturnError_WhenValidInvalidProjectSpecified()
        {
            // Arrange
            var args = new[] { "project", "build-spec", "list",
                "--project-path", "invalid.lvproj" 
                };

            // Act
            //var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().NotBe(0);
            //result.Error.Should().BeEmpty();
        }


        [Fact]
        public void ProjectBuildSpecSetVersion_ShouldExecuteWithoutErrors_WhenValidVersionProvided()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var sampleProjectPath = $"{currentDir}\\testdata\\labview\\sample.lvproj";
            // Arrange
            var args = new[] { "project", "build-spec", "set-version",
            "--project-path", $"{sampleProjectPath}",
            "--build-spec-name", "sample application",
            "--build-spec-type", "",
            "--build-spec-target", "",
            "--version", "99.88.77.66"};

            // Act
            var result = _gcd.Run(args);

            // Asssert
            //result.Return.Should().Be(0);
            //result.Error.Should().BeEmpty();
        }


        [Fact(Skip ="skip for now")]
        // [Fact]

        public void PushPull_ShouldMatch()
        {
            // Arrange
            var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = GetAzureFeedUri();

            var sourcePackageContent = Guid.NewGuid().ToString();
            var sourcePackageGzContent = Guid.NewGuid().ToString();
            var sourcePackageStampsContent = Guid.NewGuid().ToString();

            File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);


            Push(feedSourceDirectory, feedUri);
            Pull(feedDestinationDirectory, feedUri);


            var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
            var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
            var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

            destinationPackagesContent.Should().Be(sourcePackageContent);
            destinationPackagesGzContent.Should().Be(sourcePackageGzContent);
            destinationPackagesStampsContent.Should().Be(sourcePackageStampsContent);

            Directory.Delete(feedSourceDirectory, true);
            Directory.Delete(feedDestinationDirectory, true);

        }
        
        [Fact(Skip ="skip for now")]
        public void AddPackageToAZ()
        {
            // Arrange
            var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = GetAzureFeedUri();
            var pathToNipkg = "C:\\Projects\\gcd_0.5.0.123_windows_x64.nipkg";

            var sourcePackageContent = "";
            var sourcePackageGzContent = "";
            var sourcePackageStampsContent = "";

            File.WriteAllText($"{feedSourceDirectory}\\Packages", sourcePackageContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.gz", sourcePackageGzContent);
            File.WriteAllText($"{feedSourceDirectory}\\Packages.stamps", sourcePackageStampsContent);
            Push(feedSourceDirectory, feedUri);

            AddPackage(pathToNipkg, feedUri);


            Pull(feedDestinationDirectory, feedUri);


            var destinationPackagesContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages");
            var destinationPackagesGzContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.gz");
            var destinationPackagesStampsContent = File.ReadAllText($"{feedDestinationDirectory}\\Packages.stamps");

            //destinationPackagesContent.Should().Be(sourcePackageContent);
            //destinationPackagesGzContent.Should().Be(sourcePackageGzContent);
            //destinationPackagesStampsContent.Should().Be(sourcePackageStampsContent);

            Directory.Delete(feedSourceDirectory, true);
            Directory.Delete(feedDestinationDirectory, true);

        }

        [Fact]
        public void NipkgBuildPackageTest()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
            var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

            var args = _args.NipkgPackageCreate(
                packageDestinationDirectory: packageDestinationDirectory,
                packageContentDirectory: packageContentDirectory);

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
            File.Exists($"{packageDestinationDirectory}\\sample-package_99.88.77.66_windows_x64.nipkg");
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


        #region api calls

        #endregion

        private string GetAzureFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/nipkg-private-feed?sp=racwdl&st=2024-12-04T21:54:19Z&se=2024-12-05T05:54:19Z&spr=https&sv=2022-11-02&sr=c&sig=2lfHgt5HxVvz0VKyhII0IMTzUlf1lSYs9zL1MDwJZ3w%3D";

    }
}