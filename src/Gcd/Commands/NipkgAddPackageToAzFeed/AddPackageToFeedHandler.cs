using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgPullFeedMeta;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.NipkgAddPackageToAzFeed;

public record AddPackageToFeedRequest(AzBlobFeedDefinition AzFeedDef, PackagePath PackagePath) : IRequest<Result>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(
    IMediator mediator,
    IUploadAzBlobService uploadService)
    : IRequestHandler<AddPackageToFeedRequest, Result>
{
    public async Task<Result> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = await localFeedDef
            .Bind((arg) => CreteTempPackagePath(arg, packagePath));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => mediator.PullAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value))
            .Bind(() => CopyPackage(packagePath, insideFeedPkgPath.Value))
            .Bind(() => AddPackageToLcalFeed(localFeedDef.Value, insideFeedPkgPath.Value))
            .Bind(() => mediator.PushAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => UploadPackage(azFeedDef, insideFeedPkgPath.Value));
    }

    private async Task<Result<LocalFeedDefinition>> CreateTempFeedDefinition()
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string currentDirectoryPath = Environment.CurrentDirectory;


        var localFeedPath = temporaryDirectory;
        var localFeedDef = LocalDirPath.Of(localFeedPath)
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));
        return localFeedDef;
    }

    private async Task<Result> UploadPackage(AzBlobFeedDefinition azFeedDef, PackagePath packagePath)
    {
        var azblob = AzBlobFeedUri.Create(azFeedDef.Feed.Full); ;
        string nipkgUrl = CreateSubUrl(azblob.Value, packagePath.PkgName);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var filePath = LocalFilePath.Of(packagePath.Value);
        var result = await uploadService.UploadFileAsync(blobUri.Value, filePath.Value);
        return result;
    }

    private string CreateSubUrl(AzBlobFeedUri feedUri, string subPath) => 
        $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";

    private async Task<Result<PackagePath>> CreteTempPackagePath(LocalFeedDefinition feedDefinition, PackagePath sourcePackagePath) =>
        PackagePath.Create($"{feedDefinition.Feed.Value}\\{sourcePackagePath.PkgName}");
    
    private async Task<Result> AddPackageToLcalFeed(LocalFeedDefinition feedDefinition, PackagePath packagePath) =>
        await mediator.RunNipkgRequestAsync(new string[] { "feed-add-pkg", feedDefinition.Feed.Value, packagePath.Value });
    
    private async Task<Result> CopyPackage(PackagePath packageSource, PackagePath packageDestinataion) => 
        Result.Try(() => File.Copy(packageSource.Value, packageDestinataion.Value, true), ex => $"{ex.Message}");
 
}



