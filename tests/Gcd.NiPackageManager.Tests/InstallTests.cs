using FluentAssertions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using Gcd.SystemProcess;

namespace NiPackageManager.Tests;

public class InstallTests
{
    [Fact]
    public async Task  WhenInstallCalled()
    {
        var processService = new ProcessService();
        var service = new NiPackageManagerService(processService);

        var packageToInstals = new List<PackageToInstall>();
        
        packageToInstals.Add(new PackageToInstall("TestPackage", "24.3.0.49385-0+f233"));
        packageToInstals.Add(new PackageToInstall("TestPackage", "24.3.0.49385-0+f233"));

        var request = new InstallRequest(packageToInstals.AsReadOnly());
        var result = await service.InstallAsync(request);
        
        result.IsSuccess.Should().BeTrue();
    }
}