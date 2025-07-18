using FluentAssertions;
using Gcd.Handlers.Nipkg.InstallFromInstallerIso;
using Gcd.Handlers.Shared;
using Gcd.Handlers.Tools;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.Services;
using MediatR;
using Moq;

namespace Gcd.Tests.Handlers;

public class SetIniParameterHandlerTests
{
    [Fact]
    public async Task SucessCase()
    {
        var sectionName = "Section";
        var mediator = new Mock<IMediator>();
        var serializer = new Mock<SnapshotSerializerJson>();
        mediator.Setup(m => m.Send(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()));
        
        var lines = new List<string>{
            $"[{sectionName}]",
            "ExistingKey=Value",
            "ExistingKeyToReplace=ToBeReplaced",
        };


        var request = new SetIniParameterRequest(lines, sectionName, "NewKey", "NewValue", true);
        var handler = new SetIniParameterHandler();
        var result = await handler.Handle(request, CancellationToken.None);
        
        request = new SetIniParameterRequest(lines, sectionName, "ExistingKeyToReplace", "ReplaceValue", true);
        var result2 = await handler.Handle(request, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
    }
}