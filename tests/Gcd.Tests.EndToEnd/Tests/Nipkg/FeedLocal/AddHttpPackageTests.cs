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

namespace Gcd.Tests.EndToEnd.Tests.Nipkg.FeedLocal;

public class AddHttpPackageTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact]
    private void AddHttpPackageTest()
    {

        var feedDir = _tempDirectoryGenerator.GenerateTempDirectory();
        // Arrange
        string packageUri = "https://github.com/zoryatec/gcd/releases/download/v0.13.0/gcd_0.13.0_windows_x64.nipkg";

        var args = new NipkgArgBuilder()
            .WithNipkgCmd()
            .WithFeedLocalCmd()
            .WithAddHttpPackageCmd()
            .WithPackageHttpOpt(packageUri)
            .WithFeedLocalDirOpt(feedDir)
            .WithFeedCreateFlag()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert



        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);

        var uri = new Uri(packageUri);
        var packageName = Path.GetFileName(uri.AbsolutePath);
        var destinationPackagesContent = File.ReadAllText($"{feedDir}\\Packages");
        var destinationPackagesGzContent = File.ReadAllText($"{feedDir}\\Packages.gz");
        var destinationPackagesStampsContent = File.ReadAllText($"{feedDir}\\Packages.stamps");

        destinationPackagesContent.Should().Contain(packageName);
        destinationPackagesStampsContent.Should().Contain(packageName);

    }
    public string BuildPackage()
    {
        // Arrange
        var packageName = "sample-package";
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
