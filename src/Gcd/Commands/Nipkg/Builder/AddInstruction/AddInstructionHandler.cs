using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;
using Gcd.Model;
using Gcd.Services.FileSystem;
using Gcd.Tests.UnitTest;
using MediatR;



namespace Gcd.Commands.Nipkg.Builder.AddInstruction;

public record AddInstructionRequest(PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks) : IRequest<Result>;

public class AddInstructionHandler(IFileSystem _fs, IInstructionsSerializer _serial)
    : IRequestHandler<AddInstructionRequest, Result>
{
    public async Task<Result> Handle(AddInstructionRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, contentLinks) = request;
        var results = new List<Result>();

        var defR = PackageBuilderDefinition.Of(rootDir);
        if (defR.IsFailure) return defR;
        var instrFilePath = defR.Value.InstructionFile;

        var content = await _fs.ReadTextFileAsync(instrFilePath);

        var currenInstruction = _serial.Deserialize(content.Value);



        contentLinks.ForEach(async link => results.Add(await AddInstruction(rootDir, link)));
        return Result.Combine(results);
    }

    public async Task<Result> AddInstruction(PackageBuilderRootDir rootDir, ContentLink contentLink) =>
        await PackageBuilderContentDir.Of(rootDir, contentLink.TargetRootDir)
            .Bind(dir => _fs.CopyDirectoryRecursievely(contentLink.ContentSourceDir, dir));



}

public static class MediatorExtensions
{
    public static async Task<Result> AddInstructionAsync(this IMediator mediator, PackageBuilderRootDir rootDir, IReadOnlyList<ContentLink> contentLinks, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddInstructionRequest(rootDir, contentLinks), cancellationToken);
    public static async Task<Result> AddInstructionAsync(this IMediator mediator, PackageBuilderRootDir rootDir, ContentLink contentLink, CancellationToken cancellationToken = default)
    => await mediator.Send(new AddInstructionRequest(rootDir, new List<ContentLink> { contentLink }), cancellationToken);

    public static async Task<Result> AddInstructionAsync(this IMediator mediator, PackageBuilderRootDir rootDir, InatallationTargetRootDir targetRootDir, PackageBuilderContentSourceDir contentSourceDir, CancellationToken cancellationToken = default)
=> await mediator.Send(new AddInstructionRequest(rootDir, new List<ContentLink> { new ContentLink(targetRootDir, contentSourceDir) }), cancellationToken);
}