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

        [Fact]
        public void PackageFileNameTest3()
        {
            string test = "deb8acb7-c580-4bcb-bc1d-f6a2946d2966_99.88.77.66_windows_x64.nipkg";

            var result = PackageFileName.Of(test);

            result.IsFailure.Should().BeFalse();
        }



    }
}
