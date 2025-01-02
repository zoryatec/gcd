using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup;

namespace Gcd.Tests.EndToEnd.Nipkg;

public class SetConfigTests(TestFixture testFixture) : BaseTest(testFixture)
{

    //[Fact(Skip ="for now")]
    [Fact]
    public void SetConfigTest()
    {
        //Arange
        string path = "C:\\Program Files\\National Instruments\\NI Package Manager\\nipkg.exe";
        var args = (new SetConfigArgBuilder())
            .WithNipkgCmdPath(path)
            //.WithNipkgInstallerUri(NipkgInstallerUri.None.ToString())
                .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        result.Out.Should().NotBeEmpty();

        var gcd = _procFactory.Create();
        //Arange
        var args2 = (new GetConfigArgBuilder())
            .WithNipkgCmdPath()
                .Build();

        // Act
        var result2 = gcd.Run(args2);

        // Asssert
        result2.Error.Should().BeEmpty();
        result2.Return.Should().Be(0);
        result2.Out.Should().NotBeEmpty();
    }

    [Fact]
    public void GetConfigTestUnfound()
    {

        var gcd = _procFactory.Create();
        //Arange
        var args2 = (new GetConfigArgBuilder())
            .WithNipkgInstallerUri()
                .Build();

        // Act
        var result2 = gcd.Run(args2);

        // Asssert
        //result2.Error.Should().BeEmpty();
        //result2.Return.Should().Be(0);
        //result2.Out.Should().NotBeEmpty();
    }
}

