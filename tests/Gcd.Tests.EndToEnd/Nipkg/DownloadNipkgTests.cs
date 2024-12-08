
using FluentAssertions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg;

public class DownloadNipkgTests : IClassFixture<TestFixture>
{
    IGcdProcess _gcd;
    GcdArgsBuilder _args;
    ITempDirectoryGenerator _tempDirectoryGenerator;
    TestConfiguration _config;
    public DownloadNipkgTests(TestFixture testFixture)
    {
        _gcd = new GcdProcessApp();
        _args = new GcdArgsBuilder();
        _tempDirectoryGenerator = new TempDirectoryGenerator();
        _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
    }

    [Fact]
    public void NipkgDownload_ShouldDownloadFiles_WhenLinkIsValid()
    {
        // Arange
        var tempDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var localPath = $"{tempDir}\\nipkg-installer.exe";
        var args = (new DownloadNipkgArgBuilder())
                .WithLocalPath(localPath)
                .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();

        File.Exists(localPath).Should().BeTrue();

        Directory.Delete(tempDir, recursive: true);

    }
}

