//using CSharpFunctionalExtensions;
//using Gcd.Model.FeedDefinition;
//using Gcd.Model.File;
//using Gcd.Services;
//using Gcd.Services.FileSystem;
//using Gcd.Services.RemoteFileSystem;
//using MediatR;

//namespace Gcd.Handlers.Nipkg.RemoteFeed;

//public record PullFeedMetaGenericRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;

//public record PullFeedSmbMetaRequest(FeedDefinitionSmb FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;
//public record PullFeedAzBlobMetaRequest(FeedDefinitionAzBlob FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;

//public record PullFeedGenericMetaRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef)
//    : IRequest<Result> where TFeedDefinition : IFeedDefinition;

//public class PullFeedMetaGenericHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
//    : IRequestHandler<PullFeedGenericMetaRequest<FeedDefinitionSmb>, Result>
//{
//    public async Task<Result> Handle(PullFeedGenericMetaRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
//    {
//        var (azFeedDef, localFeedDef) = request;
//        return await _fs.CreateDirAsync(localFeedDef.Feed)
//            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
//            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
//            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
//    }
//}


//public static class MediatorExtensionsPulldd
//{
//    public static async Task<Result> PullFeedMetaGenAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
//        => await mediator.Send(new PullFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
//}
