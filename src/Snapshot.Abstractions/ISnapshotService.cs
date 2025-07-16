using CSharpFunctionalExtensions;

namespace Snapshot.Abstractions;


public record InstallFromSnapshotRequest(Snapshot Snapshot);
public record InstallFromSnapshotResponse(Snapshot Snapshot);


public record InstallFromInstallerDirectoryRequest(string InstallerDirectory);
public record InstallFromInstallerDirectoryResponse(string InstallerDirectory);

public record InstallFromInstallerLocalIsoRequest(string isoFilePath);
public record InstallFromInstallerLocalIsoResponse(string isoFilePath);


public record CreateSnapshotFromSystemRequest();
public record CreateSnapshotFromSystemResponse();

public interface ISnapshotService
{
    public Task<Result<InstallFromSnapshotResponse>> InstallFromSnapshotAsync(InstallFromSnapshotRequest request);
    
    public Task<Result<InstallFromInstallerDirectoryResponse>> InstallFromInstallerDirectory(InstallFromInstallerDirectoryRequest request);
    
    public Task<Result<InstallFromInstallerLocalIsoResponse>> InstallFromInstallerLocalIso(InstallFromInstallerLocalIsoRequest request);
    
    public Task<Result<CreateSnapshotFromSystemResponse>> CreateSnapshotFromSystem(CreateSnapshotFromSystemRequest request);
    
    
}