using CSharpFunctionalExtensions;
using FluentAssertions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.Handlers.Nipkg.InstallFromInstallerIso;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.SnapshotManagment;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Providers;
using Gcd.Services;
using Gcd.SystemProcess;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class InstallPackagesFromInstallerIsoTests
{
    // [Fact]
    public async Task SucessCase()
    {
        var simulation = true;
        var mediator = new Mock<IMediator>();
        var systemProcess = new ProcessService();
        var installerDirectoryProvider = new InstallerDirectoryProvider();
        var nipkgService = new NiPackageManagerService(systemProcess);
        var nipkgExtendedService = new NiPackageManagerExtendedService(nipkgService);
        var localFileSystem = new LocalFileService();
        mediator
            .Setup(m => m.Send(It.IsAny<CreateSnapshotFromInstallerRequest>(), It.IsAny<CancellationToken>()))
            .Returns((CreateSnapshotFromInstallerRequest req, CancellationToken token) => {
                var handler = new CreateFromInstallerDirectoryHandler(mediator.Object,installerDirectoryProvider);
                return handler.Handle(req, token);
            });
        
        mediator
            .Setup(m => m.Send(It.IsAny<InstallFromSnapshotRequest>(), It.IsAny<CancellationToken>()))
            .Returns((InstallFromSnapshotRequest req, CancellationToken token) => {
                var handler = new InstallFromSnapshotHandler(mediator.Object,nipkgExtendedService);
                return handler.Handle(req, token);
            });
        
        mediator
            .Setup(m => m.Send(It.IsAny<InstallFromInstallerDirectoryRequest>(), It.IsAny<CancellationToken>()))
            .Returns((InstallFromInstallerDirectoryRequest req, CancellationToken token) => {
                var handler = new InstallFromInstallerDirectoryHandler(mediator.Object);
                return handler.Handle(req, token);
            });
        
        mediator
            .Setup(m => m.Send(It.IsAny<ExpandIsoFileRequest>(), It.IsAny<CancellationToken>()))
            .Returns((ExpandIsoFileRequest req, CancellationToken token) => {
                var handler = new ExpandIsoFileHandler(mediator.Object);
                return handler.Handle(req, token);
            });
        
        var serializer = new Mock<SnapshotSerializerJson>();


        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var relativeIsoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..",
            "..", "manual-docker", "installers-iso","ni-labview-2025-community-x86_25.1.3_offline.iso");
        var fullIsoPath = Path.GetFullPath(relativeIsoPath);
        
        var outputIsoFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestIsoOutput");

        var isFilePath = LocalFilePath.Of(fullIsoPath);
        var expandDirectory = LocalDirPath.Of(outputIsoFolder);
        
        
        var request = new InstallFromInstallerIsoRequest(isFilePath.Value,expandDirectory.Value, false,
            false,
            Maybe.None, simulation);
        var handler = new InstallFromInstallerIsoHandler(mediator.Object,localFileSystem);
        var result = await handler.Handle(request, CancellationToken.None);
        
        
        Assert.True(result.IsSuccess);

    }
}