using Gcd.Model.Config;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedLocal;


public class AddPackagesFromDirectoryToLcalFeedHandler(IFileSystem _fs, IMediator _mediator)
    : IRequestHandler<AddPackagesFromDirectoryToLcalFeedRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddPackagesFromDirectoryToLcalFeedRequest request, CancellationToken cancellationToken)
    {
        var (localFeedDef, packageDir, cmdPath, createFeed, useAbsPath) = request;

        var paths = _fs.ListFiles(packageDir);
        if (!paths.Value.Any()) return UnitResult.Failure<Error>(new Error($"No packages found in {packageDir.Value}"));
        foreach (var path in paths.Value)
        {
            var path1 = PackageLocalFilePath.Of(path.Value);
                
            var result = await _mediator.AddToLocalFeedAsync(localFeedDef, path1.Value, cmdPath,useAbsPath, createFeed );
            if(result.IsFailure) return UnitResult.Failure<Error>(new Error(result.Error));
        }
        
        return UnitResult.Success<Error>();
    }
}


public record AddPackagesFromDirectoryToLcalFeedRequest(FeedDefinitionLocal FeedDefinition, ILocalDirPath PackageSourceDir, NipkgCmdPath CmdPath, bool createFeed, UseAbsolutePath UseAbsolutePath) : IRequest<UnitResult<Error>>;


public static class MediatorExtensions3
{
    public static async Task<UnitResult<Error>> AddLocalDirAsync(this IMediator mediator,
        FeedDefinitionLocal localFeedDefinition,
        ILocalDirPath localDirPath,
        NipkgCmdPath CmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new AddPackagesFromDirectoryToLcalFeedRequest(
                localFeedDefinition, 
                localDirPath,
                CmdPath,
                createFeed,
                useAbsolutePath), 
            cancellationToken);
    
}
