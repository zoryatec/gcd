using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;


namespace Gcd.Handlers.Nipkg.RemoteFeed;

public record PushFeedMetaDataRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDefinition)
        : IRequest<Result> where TFeedDefinition : IFeedDefinition;


public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<PushFeedMetaDataRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(PushFeedMetaDataRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _rfs.UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}

public static class MediatorExtensionsPush
{
    public static async Task<Result> PushFeedMetaDataAsync<TFeedDefinition>(this IMediator mediator, TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default) where TFeedDefinition : IFeedDefinition
        => await mediator.Send(new PushFeedMetaDataRequest<TFeedDefinition>(FeedDefinition, LocalFeedDef), cancellationToken);
}