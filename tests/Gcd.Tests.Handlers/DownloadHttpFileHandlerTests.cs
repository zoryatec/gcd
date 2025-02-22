using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.Services;
using Gcd.Tests.Fixture;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Gcd.Tests.Handlers;

public class DownloadHttpFileHandlerTests(TestFixture testFixture) : IClassFixture<TestFixture>
{
    [Fact]
    public async Task Test1()
    {
        var mediator = new Mock<IMediator>();
        var webDownload = new Mock<IWebDownload>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        var handler = new DownloadHttpFileHandler(webDownload.Object);

        var localFilePath = LocalFilePath.Of("C:\test.txt");
        var webUri = WebFileUri.Of("http://localhost:5000/test.txt");
        var request = new DownloadHttpFileRequest(localFilePath.Value, webUri.Value);
        var result = await handler.Handle(request, CancellationToken.None);
    }
}