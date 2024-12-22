using FluentAssertions;
using Gcd.Model.Config;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg;

public class SetConfigTests : IClassFixture<TestFixture>
{
    IGcdProcess _gcd;
    GcdArgsBuilder _args;
    ITempDirectoryGenerator _tempDirectoryGenerator;
    TestConfiguration _config;
    public SetConfigTests(TestFixture testFixture)
    {
        _gcd = new GcdProcessApp();
        _args = new GcdArgsBuilder();
        _tempDirectoryGenerator = new TempDirectoryGenerator();
        _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
    }

    //[Fact(Skip ="for now")]
    [Fact]
    public void SetConfigTest()
    {
        //Arange
        var args = (new SetConfigArgBuilder())
            .WithNipkgCmdPath("C:\\Program Files\\National Instruments\\NI Package Manager\\nipkg.exe")
            //.WithNipkgInstallerUri(NipkgInstallerUri.None.ToString())
                .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
        result.Out.Should().NotBeEmpty();

        _gcd = new GcdProcessApp();
        //Arange
        var args2 = (new GetConfigArgBuilder())
            .WithNipkgCmdPath()
                .Build();

        // Act
        var result2 = _gcd.Run(args2);

        // Asssert
        result2.Error.Should().BeEmpty();
        result2.Return.Should().Be(0);
        result2.Out.Should().NotBeEmpty();
    }

    [Fact]
    public void GetConfigTestUnfound()
    {

        _gcd = new GcdProcessApp();
        //Arange
        var args2 = (new GetConfigArgBuilder())
            .WithNipkgInstallerUri()
                .Build();

        // Act
        var result2 = _gcd.Run(args2);

        // Asssert
        //result2.Error.Should().BeEmpty();
        //result2.Return.Should().Be(0);
        //result2.Out.Should().NotBeEmpty();
    }
}

