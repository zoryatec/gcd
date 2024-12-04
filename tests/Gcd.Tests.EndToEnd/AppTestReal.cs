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
        ITempDirectoryGenerator _tempFolderGenerator;
        public  AppTestReal()
        {
            _gcd = new GcdProcess();
            _tempFolderGenerator = new TempDirectoryGenerator();
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

        [Fact]
        public void SystemAddToUserPath()
        {
            // Arrange
            var args = new[] { "system", "add-to-user-path", $"C:\\{Guid.NewGuid().ToString()}" };

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }

    }
}
