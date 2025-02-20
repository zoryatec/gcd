using FluentAssertions;
using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu;
using Gcd.Tests.EndToEnd.Setup;
using Gcd.Tests.Fixture;

namespace Gcd.Tests.EndToEnd.LabView.LabView;

public class KillTests(TestFixture testFixture) : BaseTest(testFixture)
{
    [Fact]
    public void KillTest()
    {
        // Arrange
        var args = GcdArgBuilder.Create()
            .WithLabViewMenu()
            .WithKillCmd()
            .Build();

        // Act
        var result = _gcd.Run(args);

        // Asssert
        result.Error.Should().BeEmpty();
        result.Return.Should().Be(0);
    }
}