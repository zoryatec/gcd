using FluentAssertions;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.Tools
{
    public class AddToUserPathTests(TestFixture testFixture) : BaseTest(testFixture)
    {

        [Fact]
        public void SystemAddToUserPath()
        {
            // Arrange

            var args =  new GcdArgBuilder()
                .WithToolsMenu()
                .WithAddToUserPathCmd()
                .WithPath($"C:\\gcd-add-test-path")
                .Build();

            // Act
            var result = _gcd.Run(args);

            // Assert
            Assert.Multiple(() =>
            {
                result.Error.Should().BeEmpty();
                result.Return.Should().Be(0);   
            });
        }
    }
}
