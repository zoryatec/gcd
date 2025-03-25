using FluentAssertions;
using Gcd.DI;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using Gcd.Services.FileSystem;
using Gcd.Tests.Fixture;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Gcd.Tests.Handlers;

public class DownloadArchiveHandlerTest
{
    [Fact]
    public async Task Test_DownloadArchiveHandler()
    {
        var console = new FakeConsole();
        var assembly = typeof(Program).Assembly;
        var services = new ServiceCollection()
            .AddGcd(assembly, console);

        var serviceProvider = services.BuildServiceProvider();
        
        // --destination-dir
        // --archive-uri
        // --archive-format (in the future work out from file)
        // --archive-dir (default empty, relative path inside archive which should be moved to --destination-dir
        // overwrite files (flag to say if files should be overriden
        // create directory
        // add to path
        var url = "https://github.com/rclone/rclone/releases/download/v1.69.1/rclone-v1.69.1-windows-amd64.zip";
        var relativeContentDir = RelativeDirPath.Of("rclone-v1.69.1-windows-amd64");
        var webFileUri = WebFileUri.Of(url);
        
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var webDownload = serviceProvider.GetRequiredService<IWebDownload>();
        
        var fileSystem = serviceProvider.GetRequiredService<IFileSystem>();
        var tempDir = await fileSystem.GenerateTempDirectoryAsync();
        await fileSystem.CreateDirAsync(tempDir.Value);
        var handler = new DownloadArchiveHandler(mediator, fileSystem);

        var request = new DownloadArchiveRequest(
            webFileUri.Value,
            relativeContentDir.Value,
            tempDir.Value
        );
        
        
        var result = await handler.Handle(request, CancellationToken.None);

        File.Exists($"{tempDir.Value}\\rclone.exe").Should().BeTrue();
    }
}