using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Tools;

public class DownloadNipkgTests(TestFixture testFixture) : BaseTest(testFixture)
{

    //[Fact(Skip ="skip for now, no two web downloads runs well, race condition?")]
    //[Fact]
    public void NipkgDownload_ShouldDownloadFiles_WhenLinkIsValid()
    {
        // Arange
        var tempDir = _tempDirectoryGenerator.GenerateTempDirectory();
        var localPath = $"{tempDir}\\nipkg-installer.exe";
        var args = new GcdArgBuilder()
            .WithToolsMenu()
            .WithDownloadNipkgCmd()
            .WithLocalPath(localPath)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);


        File.Exists(localPath).Should().BeTrue();

        Directory.Delete(tempDir, recursive: true);

    }
}

