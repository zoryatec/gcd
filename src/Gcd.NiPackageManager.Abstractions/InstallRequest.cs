namespace Gcd.NiPackageManager.Abstractions;

public record InstallRequest( IReadOnlyList<PackageToInstall> PackagesToInstalls, bool AcceptEulas = false, 
    bool AssumeYes = false, bool Simulate = false, bool ForceLocked = false, bool SuppressIncompatibilityErrros = false,
    bool Verbose = false );