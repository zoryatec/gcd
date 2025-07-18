using FluentAssertions;
using Gcd.Handlers.Nipkg.InstallFromInstallerIso;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.Services;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class ExpandIsoFileTests
{
    [Fact]
    public async Task SucessCase()
    {
        var mediator = new Mock<IMediator>();
        var serializer = new Mock<SnapshotSerializerJson>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var relativeIsoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..",
            "..", "manual-docker", "installers-iso","ni-labview-2025-community-x86_25.1.3_offline.iso");
        var fullIsoPath = Path.GetFullPath(relativeIsoPath);
        
        var outputIsoFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestIsoOutput");

        var isFilePath = LocalFilePath.Of(fullIsoPath);
        var expandDirectory = LocalDirPath.Of(outputIsoFolder);

        var request = new ExpandIsoFileRequest(isFilePath.Value, expandDirectory.Value, false);
        var handler = new ExpandIsoFileHandler(mediator.Object);
        var result = await handler.Handle(request, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }
}