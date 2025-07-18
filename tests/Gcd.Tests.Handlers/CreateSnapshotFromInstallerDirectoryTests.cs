using FluentAssertions;
using Gcd.Handlers.Nipkg.SnapshotManagment;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.Services;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class CreateSnapshotFromInstallerDirectoryTests
{
    [Fact(Skip ="for now")]
    public async Task SucessCase()
    {
        var mediator = new Mock<IMediator>();
        var serializer = new Mock<SnapshotSerializerJson>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var testInstallerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "TestInstaller");
        var ouptutFilePathRaw = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshot.json");
        
        var installerDirectory = LocalDirPath.Of(testInstallerPath);
        var outputFilePath = LocalFilePath.Of(ouptutFilePathRaw);
        var request = new CreateSnapshotFromInstallerRequest(installerDirectory.Value);
        var handler = new CreateFromInstallerDirectoryHandler(mediator.Object);
        var result = await handler.Handle(request, CancellationToken.None);
        
        

        Assert.True(result.IsSuccess, "Snapshot creation failed");
        
        var snapshot = result.Value.Snapshot;

        snapshot.Feeds.Count.Should().Be(2);
        snapshot.Packages.Count.Should().Be(10);

    }
}