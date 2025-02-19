using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Config;

public class SetConfigTests(TestFixture testFixture) : BaseTest(testFixture)
{

    //[Fact(Skip ="for now")]
    [Fact]
    public void SetConfigTest()
    {
        //Arange
        string path = "C:\\Program Files\\National Instruments\\NI Package Manager\\nipkg.exe";
        var args = GcdArgBuilder.Create()
            .WithConfigMenu()
            .WithSetCmd()
            .WithNipkgCmdPath(path)
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        result.Out.Should().NotBeEmpty();

        var gcd = _procFactory.Create();
        //Arange
        var args2 = GcdArgBuilder.Create()
            .WithConfigMenu()
            .WithGetCmd()
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
        var args2 = GcdArgBuilder.Create()
            .WithConfigMenu()
            .WithGetCmd()
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

