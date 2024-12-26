
using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Tools;

public class InstallNipkgTests(TestFixture testFixture) : BaseTest(testFixture)
{

    [Fact(Skip = "for now")]
    public void NipkgInstall_ShouldSucceed_WhenLinkIsValid()
    {
        //Arange
        var args = new InstallNipkgArgBuilder()
                .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();
        result.Out.Should().NotBeEmpty();
    }
}

