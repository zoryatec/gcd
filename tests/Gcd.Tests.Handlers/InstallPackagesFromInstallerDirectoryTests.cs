using FluentAssertions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.Services;
using Gcd.Snapshot;
using Gcd.SystemProcess;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class InstallPackagesFromInstallerDirectoryTests
{
    [Fact]
    public async Task SucessCase()
    {
        var mediator = new Mock<IMediator>();
        // mediator
        //     .Setup(m => m.Send(It.IsAny<CreateSnapshotFromInstallerRequest>(), It.IsAny<CancellationToken>()))
        //     .Returns((IRequest request, CancellationToken token) => {
        //         var handler = new CreateFromInstallerDirectoryHandler(mediator.Object);
        //         return handler.Handle((CreateSnapshotFromInstallerRequest)request, token);
        //     });
        mediator
            .Setup(m => m.Send(It.IsAny<CreateSnapshotFromInstallerRequest>(), It.IsAny<CancellationToken>()))
            .Returns((CreateSnapshotFromInstallerRequest req, CancellationToken token) => {
                var handler = new CreateFromInstallerDirectoryHandler(mediator.Object);
                return handler.Handle(req, token);
            });
        var serializer = new Mock<SnapshotSerializerJson>();

        var systemProcess = new ProcessService();
        var nipkgService = new NiPackageManagerService(systemProcess);
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var testInstallerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "TestInstaller");
        var ouptutFilePathRaw = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshot.json");
        string matchPattern = "mycompany-myproduct";
        var installerDirectory = LocalDirPath.Of(testInstallerPath);
        var outputFilePath = LocalFilePath.Of(ouptutFilePathRaw);
        var request = new InstallFromInstallerDirectoryRequest(installerDirectory.Value,matchPattern);
        var handler = new InstallFromInstallerDirectoryHandler(mediator.Object,nipkgService);
        var result = await handler.Handle(request, CancellationToken.None);
        
        

        Assert.True(result.IsSuccess, result.Error);

    }
}