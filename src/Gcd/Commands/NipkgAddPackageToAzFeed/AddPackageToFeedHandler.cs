using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgPullFeedMeta;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Model;
using Gcd.Services;
using MediatR;
using System.Threading;

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
        var (azFeedDef, PackagePath) = request;

        var localFeedDef = await CreateTempFeedDefinition();
        var localFeedPath = localFeedDef.Value.Feed.Value;
        string packageName = Path.GetFileName(request.PackagePath.Value);

        var packageDestinationPath = Path.Combine(localFeedPath, packageName);


        var downloadResult = await mediator.PullAzBlobFeedMetaDataAsync(request.AzFeedDef, localFeedDef.Value);

        File.Copy(request.PackagePath.Value, packageDestinationPath, true);

        var addPakcgResult = await AddPackageToLcalFeed(localFeedPath, packageDestinationPath);






        var pushResult = await mediator.PushAzBlobFeedMetaDataAsync(request.AzFeedDef, localFeedDef.Value, cancellationToken);

        if (pushResult.IsFailure) return pushResult;



        var azblob = AzBlobFeedUri.Create(request.AzFeedDef.Feed.Full); ;
        return await UploadPackage(azblob.Value, localFeedDef.Value, packageName);
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

    private async Task<Result> UploadPackage(AzBlobFeedUri feedUri, LocalFeedDefinition locFeedDef, string packageName)
    {
        string nipkgUrl = CreateSubUrl(feedUri, packageName);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var filePath = LocalFilePath.Of($"{locFeedDef.Feed.Value}\\{packageName}");
        var result = await uploadService.UploadFileAsync(blobUri.Value, filePath.Value);
        return result;
    }

    private string CreateSubUrl(AzBlobFeedUri feedUri, string subPath)
    {
        return $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";
    }

    private async Task<Result> AddPackageToLcalFeed(string feedDir, string packagePath)
    {

        var arguments = new string[] { "feed-add-pkg", feedDir, packagePath };
        var req = new RunNipkgRequest(arguments);
        return  await mediator.Send(req);
    }

}



