
using FluentAssertions;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg;

public class InstallNipkgTests : IClassFixture<TestFixture>
{
    IGcdProcess _gcd;
    GcdArgsBuilder _args;
    ITempDirectoryGenerator _tempDirectoryGenerator;
    TestConfiguration _config;
    public InstallNipkgTests(TestFixture testFixture)
    {
        _gcd = new GcdProcessApp();
        _args = new GcdArgsBuilder();
        _tempDirectoryGenerator = new TempDirectoryGenerator();
        _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
    }

    [Fact(Skip = "for now")]
    public void NipkgInstall_ShouldSucceed_WhenLinkIsValid()
    {
        //Arange
       var args = (new InstallNipkgArgBuilder())
               .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Return.Should().Be(0);
        result.Error.Should().BeEmpty();
        result.Out.Should().NotBeEmpty();
    }
}

