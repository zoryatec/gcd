﻿using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class AddContentTests(TestFixture testFixture) : BaseTest(testFixture)
    {
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
