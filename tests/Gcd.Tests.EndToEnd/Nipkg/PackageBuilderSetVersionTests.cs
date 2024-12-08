using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Nipkg
{
    public class PackageBuilderSetVersionTests
    {
        IGcdProcess _gcd;
        GcdArgsBuilder _args;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        TestConfiguration _config;
        public PackageBuilderSetVersionTests(TestFixture testFixture)
        {
            _gcd = new GcdProcessApp();
            _args = new GcdArgsBuilder();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
            _config = testFixture.ServiceProvider.GetRequiredService<TestConfiguration>();
        }

    }
}
