using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Tests.Fixture;

public class BaseTest : IClassFixture<TestFixture>
{
    protected readonly TempDirectoryGenerator _tempDirectoryGenerator;
    protected readonly TestConfiguration _config;
    protected readonly IGcdProcess _gcd;
    protected readonly IGcdProcessFactory _procFactory;

    public BaseTest(TestFixture testFixture)
    {
        // _gcd = new GcdProcessApp();
        _tempDirectoryGenerator = new TempDirectoryGenerator();
        _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        _procFactory = testFixture.ServiceProvider.GetRequiredService<IGcdProcessFactory>();
        _gcd = _procFactory.Create();
    }
}