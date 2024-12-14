using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuilserSetVersion;

public record PackageBuilderSetPropertyRequest(PackageBuilderRootDir PackagePath, IReadOnlyList<ControlFileProperty> Properties) : IRequest<Result>;

public class PackageBuilderSetPropertyHandler()
    : IRequestHandler<PackageBuilderSetPropertyRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderSetPropertyRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, properties) = request;
        var pckDefinition = PackageBuilderDefinition.Of(rootDir);

        return  await pckDefinition
            .Bind(def => ReadTextFileAsync(def.ControlFile))
            .Bind(fileContent => ControlFileContent.Of(fileContent))
            .Map(controlFile => controlFile.WithProperties(properties))
            .Bind(controlFile => WriteTextFile(pckDefinition.Value.ControlFile, controlFile.Content));
    }
 
    private async Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default) =>
        ReadTextFile(filePath);

    private Result<string> ReadTextFile(LocalFilePath filePath) =>
        Result.Try(() => File.ReadAllText(filePath.Value), ex => ex.Message);

    private async Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default) =>
        WriteTextFile(filePath, content);

    private Result WriteTextFile(LocalFilePath filePath, string content) =>
        Result.Try(() => File.WriteAllText(filePath.Value, content), ex => ex.Message);

}

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderSetPropertiesAsync(this IMediator mediator, PackageBuilderRootDir packageBuilderRootDir, IReadOnlyList<ControlFileProperty> properties, CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, properties), cancellationToken);
    public static async Task<Result> PackageBuilderSetPropertyAsync(this IMediator mediator, PackageBuilderRootDir packageBuilderRootDir, ControlFileProperty property, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, new List<ControlFileProperty> { property }), cancellationToken);
}
