
using FluentAssertions;
using Gcd.Tests.EndToEnd.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcd.CommandBuilder.Command.Tools;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Tools;

public class InstallNipkgTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact(Skip = "for now")]
    public void NipkgInstall_ShouldSucceed_WhenLinkIsValid()
    {
        //Arange
        var args = new GcdArgBuilder()
            .WithToolsMenu()
            .WithInstallNipkgCmd()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();
        result.Out.Should().NotBeEmpty();
    }
}

