using FluentAssertions;
using Gcd.LabViewProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd
{
    public class AppTestReal
    {
        IGcdProcess _gcd;
        ITempDirectoryGenerator _tempDirectoryGenerator;
        public  AppTestReal()
        {
            //_gcd = new GcdProcess();
            _gcd = new GcdProcessApp();
            _tempDirectoryGenerator = new TempDirectoryGenerator();
        }


        [Fact]
        public void VersionTest()
        {
            // Arrange
            var args = new[] { "--version" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }
    
    }
}
