using FluentAssertions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests
{
    public class PackageFileNameTests
    {
        [Fact]
        public void PackageFileNameTest1()
        {
            string test = null;

            var result = PackageFileName.Of(test);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void PackageFileNameTest2()
        {
            string test = "dupa";

            var result = PackageFileName.Of(test);

            result.IsFailure.Should().BeTrue();
        }
    }
}
