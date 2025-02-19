using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.Nipkg.Instructions.Model;
using Gcd.Tests.UnitTest;
using MediatR;



namespace Gcd.Handlers.Nipkg.Builder;

public record AddInstructionRequest(BuilderRootDir rootDir, FilePackageCustomeExecute customExecute) : IRequest<Result>;

public class AddInstructionHandler(IFileSystem _fs, IInstructionsSerializer _serial)
    : IRequestHandler<AddInstructionRequest, Result>
{
    public async Task<Result> Handle(AddInstructionRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, customExecute) = request;
        var results = new List<Result>();

        var defR = PackageBuilderDefinition.Of(rootDir);
        if (defR.IsFailure) return defR;
        var instrFilePath = defR.Value.InstructionFile;

        return await _fs.ReadTextFileAsync(instrFilePath)
            .Bind(content => _serial.Deserialize(content))
            .Bind(model => model.AddCustomExecute(customExecute))
            .Bind(model => _serial.Serialize(model))
            .Bind(content => _fs.WriteTextFileAsync(instrFilePath, content));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> AddInstructionAsync(this IMediator mediator, BuilderRootDir rootDir, FilePackageCustomeExecute customExecute, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddInstructionRequest(rootDir, customExecute), cancellationToken);
}