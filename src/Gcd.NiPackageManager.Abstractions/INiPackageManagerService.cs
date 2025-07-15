using CSharpFunctionalExtensions;

namespace Gcd.NiPackageManager.Abstractions;

public interface INiPackageManagerService
{
    public Task<Result> Install(InstallRequest request);
    
    public Task<Result<InfoInstalledResponse>> InfoInstalledAsync(InfoInstalledRequest request);
    public Task<Result> VersionAsync();
    
}