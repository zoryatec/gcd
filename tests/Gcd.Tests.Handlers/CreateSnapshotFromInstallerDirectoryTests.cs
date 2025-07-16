using Gcd.Handlers.Nipkg.CreateSnapshotFromInstallerDirectory;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class CreateSnapshotFromInstallerDirectoryTests
{
    [Fact]
    public async Task SucessCase()
    {
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var testInstallerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "TestInstaller");
        
        var installerDirectory = LocalDirPath.Of(testInstallerPath);
        var request = new CreateSnapshotFromInstallerRequest(installerDirectory.Value);
        var handler = new CreateSnapshotFromInstallerDirectoryHandler(mediator.Object);
        var result = await handler.Handle(request, CancellationToken.None);
    }
}