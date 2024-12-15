using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Services;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Model;
using MediatR;

namespace Gcd.Commands.NipkgAddPackageToAzFeed;

public record AddPackageToFeedRequest(AzBlobFeedDefinition AzFeedDef, PackageFilePath PackagePath) : IRequest<Result>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(
    IMediator _mediator,
    IUploadAzBlobService _uploadService,
    IFileSystem _fs)
    : IRequestHandler<AddPackageToFeedRequest, Result>
{
    public async Task<Result> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = await localFeedDef
            .Bind((arg) => CreteTempPackagePath(arg, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value))
            .Bind(() =>  _fs.CopyFileAsync(packagePath, insideFeedPkgPath.Value,true))
            .Bind(() => _mediator.AddPackageToLcalFeedAsync(localFeedDef.Value, insideFeedPkgPath.Value))
            .Bind(() => _mediator.PushAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => UploadPackage(azFeedDef, insideFeedPkgPath.Value));
    }

    private async Task<Result<LocalFeedDefinition>> CreateTempFeedDefinition()
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string currentDirectoryPath = Environment.CurrentDirectory;


        var localFeedPath = temporaryDirectory;
        var localFeedDef = LocalDirPath.Parse(localFeedPath)
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));
        return localFeedDef;
    }

    private async Task<Result> UploadPackage(AzBlobFeedDefinition azFeedDef, PackageFilePath packagePath)
    {
        var azblob = AzBlobFeedUri.Create(azFeedDef.Feed.Full); ;
        string nipkgUrl = CreateSubUrl(azblob.Value, packagePath.FileName.Value);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var filePath = LocalFilePath.Offf(packagePath.Value);
        var result = await _uploadService.UploadFileAsync(blobUri.Value, filePath.Value);
        return result;
    }

    private string CreateSubUrl(AzBlobFeedUri feedUri, string subPath) => 
        $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";

    private async Task<Result<PackageFilePath>> CreteTempPackagePath(LocalFeedDefinition feedDefinition, PackageFileName srcName) =>
        PackageFilePath.Of(feedDefinition.Feed, srcName);
    

    
    private async Task<Result> CopyPackage(PackagePath packageSource, PackagePath packageDestinataion) => 
        Result.Try(() => File.Copy(packageSource.Value, packageDestinataion.Value, true), ex => $"{ex.Message}");
 
}



