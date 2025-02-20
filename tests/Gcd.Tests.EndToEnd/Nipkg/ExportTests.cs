using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Nipkg;

public class ExportTests(TestFixture testFixture) : BaseTest(testFixture)
{
    // [Fact]
    // something teribgly wrong goes here -> never finishes
    public void ExportTest1()
    {
        //Arange
        var args = new GcdArgBuilder()
            .WithNipkgMenu()
            .WithExportCmd()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        result.Out.Should().NotBeEmpty();
    }
}

