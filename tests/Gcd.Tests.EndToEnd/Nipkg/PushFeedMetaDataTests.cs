﻿using FluentAssertions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PushFeedMetaDataTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PushFeedMetaDataTests()
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = new TestConfiguration();
        }

        [Fact]
        public void Push_ShouldReturnEror_WhenPathIsEmpty()
        {
            var args = new[] {
                "nipkg", "push-feed-meta",
                "--feed-local-path", "dd",
                "--feed-uri", "dd"
                };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(1);
            result.Error.Should().NotBeEmpty();
        }

        [Fact]
        public void PushPull_ShouldMatch()
        {
            // Arrange
            var feedSourceDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedDestinationDirectory = _tempDirectoryGenerator.GenerateTempDirectory();
            var feedUri = _config.GetAzureAddPkgTestFeedUri();

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

    }
}