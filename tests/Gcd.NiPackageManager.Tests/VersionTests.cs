using FluentAssertions;
using Gcd.NiPackageManager;
using Gcd.SystemProcess;

namespace NiPackageManager.Tests;

public class VersionTests
{
    [Fact]
    public async Task  WhenVersionCalledThenReturnsVersion()
    {
        var processService = new ProcessService();
        var service = new NiPackageManagerService(processService);

        var result = await service.VersionAsync();
        
        result.IsSuccess.Should().BeTrue();
    }
}