using FluentAssertions;
using Gcd.LocalFileSystem.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests
{
    public class LocalFilePathTests
    {
        [Fact]
        public void LocFilePathTest1()
        {
            string test = null;

            var result = LocalFilePath.Of(test);

            result.IsFailure.Should().BeTrue();
        }
    }
}
