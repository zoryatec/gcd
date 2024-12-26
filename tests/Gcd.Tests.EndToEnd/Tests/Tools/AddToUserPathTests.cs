using FluentAssertions;
using Gcd.Model;
using Gcd.Tests.EndToEnd.Arguments.Nipkg;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.System
{
    public class AddToUserPathTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        [Fact(Skip ="for now")]
        public void SystemAddToUserPath()
        {
            // Arrange

            var args = (new AddToUserPathArgBuilder())
                .WithPath($"C:\\{Guid.NewGuid().ToString()}")
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Asssert
            result.Return.Should().Be(0);
            result.Error.Should().BeEmpty();
        }
    }
}
