using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.ControlFile;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.Nipkg.Builder;

public record PackageBuilderSetPropertyRequest(BuilderRootDir PackagePath, IReadOnlyList<ControlFileProperty> Properties) : IRequest<Result>;

public class PackageBuilderSetPropertyHandler(IFileSystem _fs)
    : IRequestHandler<PackageBuilderSetPropertyRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderSetPropertyRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, properties) = request;
        var pckDefinition = PackageBuilderDefinition.Of(rootDir);

        return await pckDefinition
            .Bind(def => _fs.ReadTextFileAsync(def.ControlFile))
            .Bind(fileContent => ControlFileContent.Of(fileContent))
            .Map(controlFile => controlFile.WithProperties(properties))
            .Bind(controlFile => _fs.WriteTextFileAsync(pckDefinition.Value.ControlFile, controlFile.Content));
    }
}
