﻿using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;
using Gcd.Tests.EndToEnd.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Tests.Nipkg.FeedGit;

public class AddLocalPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    private void AddLocalPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var feedUri = _config.GetAzureAddPkgTestFeedUri();
        // Arrange
        string packagePath = BuildPackage();
        string repoAddress = _config.GetGitRepoAddress();
        string username = _config.GetGitUserName();
        string password = _config.GetGitPassword();
        string committerName = "GCD Test";
        string committerEmail = "gcd@zoratec.com";

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedGitCmd()
            .WithAddLocalPackageCmd()
            .WithGitRepoAddressOpt(repoAddress)
            .WithGitUserNameOpt(username)
            .WithGitPasswordOpt(password)
            .WithGitCommitterNameOpt(committerName)
            .WithGitCommitterEmailOpt(committerEmail)
            .WithPackageLocalPathOpt(packagePath)
            .WithFeedCreateFlag()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        //var packageName = Path.GetFileName(packagePath);
        //var destinationPackagesContent = File.ReadAllText($"{feedDir}\\Packages");
        //var destinationPackagesGzContent = File.ReadAllText($"{feedDir}\\Packages.gz");
        //var destinationPackagesStampsContent = File.ReadAllText($"{feedDir}\\Packages.stamps");

        //destinationPackagesContent.Should().Contain(packageName);
        //destinationPackagesStampsContent.Should().Contain(packageName);

    }
    public string BuildPackage()
    {
        // Arrange
        var packageName = Guid.NewGuid().ToString();
        var packageVersion = "99.88.77.66";
        var packageInstalationDir = "BootVolume/Zoryatec/sample-package";

        var packageContentDirectory = GetPackageContentDir();
        var packageDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();

        var args = new PackageBuildArgBuilder()
            .WithPackageContentDirectory(packageContentDirectory)
            .WithPackageName(packageName)
            .WithPackageVersion(packageVersion)
            .WithPackageInstalationDir(packageInstalationDir)
            .WithPackageDestinationDir(packageDestinationDirectory)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        var packagePath = $"{packageDestinationDirectory}\\{packageName}_{packageVersion}_windows_x64.nipkg";
        File.Exists(packagePath).Should().BeTrue();
        return packagePath;
    }
    private string GetPackageContentDir()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var packageContentDirectory = Path.Combine(currentDir, "testdata", "nipkg", "test-pkg-content");
        return packageContentDirectory;
    }
}
