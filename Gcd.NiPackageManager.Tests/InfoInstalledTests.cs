using FluentAssertions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using Gcd.SystemProcess;

namespace NiPackageManager.Tests;

public class InfoInstalledTests
{
    [Fact]
    public async Task  WhenInstallCalled()
    {
        var processService = new ProcessService();
        var service = new NiPackageManagerService(processService);

        var packageToInstals = new List<PackageToInstall>();

        var request = new InfoInstalledRequest("ni-package*");

        var result = await service.InfoInstalledAsync(request);
        
        result.IsSuccess.Should().BeTrue();
    }
}