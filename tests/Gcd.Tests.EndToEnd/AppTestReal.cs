using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class AppTestReal
    {
        IGcdProcess _gcd;
        public  AppTestReal()
        {
            _gcd = new GcdProcess();
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
            result.Error.Should().BeEmpty("0.");
        }

    }
}
