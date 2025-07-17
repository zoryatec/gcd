using CSharpFunctionalExtensions;
using FluentAssertions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Services;
using Gcd.Snapshot;
using Gcd.SystemProcess;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class InstallPackagesFromInstallerDirectoryTests
{
    [Fact(Skip ="for now")]
    public async Task SucessCase()
    {
        var simulation = true;
        var mediator = new Mock<IMediator>();
        var systemProcess = new ProcessService();
        var nipkgService = new NiPackageManagerService(systemProcess);
        mediator
            .Setup(m => m.Send(It.IsAny<CreateSnapshotFromInstallerRequest>(), It.IsAny<CancellationToken>()))
            .Returns((CreateSnapshotFromInstallerRequest req, CancellationToken token) => {
                var handler = new CreateFromInstallerDirectoryHandler(mediator.Object);
                return handler.Handle(req, token);
            });
        
        mediator
            .Setup(m => m.Send(It.IsAny<InstallFromSnapshotRequest>(), It.IsAny<CancellationToken>()))
            .Returns((InstallFromSnapshotRequest req, CancellationToken token) => {
                var handler = new InstallFromSnapshotHandler(mediator.Object,nipkgService);
                return handler.Handle(req, token);
            });
        
        var serializer = new Mock<SnapshotSerializerJson>();


        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var testInstallerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "TestInstaller");
        var ouptutFilePathRaw = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshot.json");
        string matchPattern = "mycompany-myproduct";
        var installerDirectory = LocalDirPath.Of(testInstallerPath);
        var outputFilePath = LocalFilePath.Of(ouptutFilePathRaw);
        var request = new InstallFromInstallerDirectoryRequest(installerDirectory.Value,Maybe.None, simulation);
        var handler = new InstallFromInstallerDirectoryHandler(mediator.Object,nipkgService);
        var result = await handler.Handle(request, CancellationToken.None);
        
        
        var packagesToRemove = new List<PackageToInstall>
        {
            new PackageToInstall("mycompany-myproduct", ""),
        };
        var removeRequest = new RemoveRequest(packagesToRemove, true, simulation, true, false, true);
        var resultRemove = await nipkgService.RemoveAsync(removeRequest);

        if (result.IsFailure)
        {
            throw new Exception(result.Error);
        }
        Assert.True(result.IsSuccess);

    }
}