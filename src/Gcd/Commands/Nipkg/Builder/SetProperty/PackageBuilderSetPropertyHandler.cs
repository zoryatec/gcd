﻿using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services.FileSystem;
using MediatR;

namespace Gcd.Commands.Nipkg.Builder.SetProperty;

public record PackageBuilderSetPropertyRequest(PackageBuilderRootDir PackagePath, IReadOnlyList<ControlFileProperty> Properties) : IRequest<Result>;

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

public static class MediatorExtensions
{
    public static async Task<Result> PackageBuilderSetPropertiesAsync(this IMediator mediator, PackageBuilderRootDir packageBuilderRootDir, IReadOnlyList<ControlFileProperty> properties, CancellationToken cancellationToken = default)
        => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, properties), cancellationToken);
    public static async Task<Result> PackageBuilderSetPropertyAsync(this IMediator mediator, PackageBuilderRootDir packageBuilderRootDir, ControlFileProperty property, CancellationToken cancellationToken = default)
    => await mediator.Send(new PackageBuilderSetPropertyRequest(packageBuilderRootDir, new List<ControlFileProperty> { property }), cancellationToken);
}
