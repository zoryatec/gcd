using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Model;
using Gcd.Services;
using MediatR;
using System.Threading;

namespace Gcd.Commands.NipkgAddPackageToAzFeed;

public record PackagePath
{
    public static Result<PackagePath> Create(Maybe<string> packagePathOrNothing)
    {
        return packagePathOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "Package path should not be empty")
            .Map(feedUri => new PackagePath(feedUri));
    }

    private PackagePath(string path) => Value = path;
    public string Value { get; }
}

 public record AddPackageToFeedRequest(AzBlobFeedDefinition AzFeedDef, PackagePath PackagePath) : IRequest<Result>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(
    IMediator mediator,
    IUploadAzBlobService uploadService)
    : IRequestHandler<AddPackageToFeedRequest, Result>
{
    public async Task<Result> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string currentDirectoryPath = Environment.CurrentDirectory;


        var localFeedPath = temporaryDirectory;
        var localFeedDef = LocalDirPath.Of(localFeedPath)
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));
        var downloadReq = new NipkgPullFeedMetaRequest(request.AzFeedDef, localFeedDef.Value);
        string packageName = Path.GetFileName(request.PackagePath.Value);
        var packageDestinationPath = Path.Combine(localFeedPath, packageName);

        var downloadResult = await mediator.Send(downloadReq);

        File.Copy(request.PackagePath.Value, packageDestinationPath, true);

        var addPakcgResult = await AddPackageToLcalFeed(localFeedPath, packageDestinationPath);



        var localFeedDef3 = LocalDirPath.Of(localFeedPath)
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));


        var pushResult = await mediator.PushAzBlobFeedMetaDataAsync(request.AzFeedDef, localFeedDef3.Value, cancellationToken);

        if (pushResult.IsFailure) return pushResult;

        //Directory.Delete(temporaryDirectory, true);


        var azblob = AzBlobFeedUri.Create(request.AzFeedDef.Feed.Full); ;
        return await UploadPackage(azblob.Value, FeedPath.Create(localFeedPath).Value, packageName);
    }

    private async Task<Result> UploadPackage(AzBlobFeedUri feedUri, FeedPath localFeedPath, string packageName)
    {
        string nipkgUrl = CreateSubUrl(feedUri, packageName);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var filePath = LocalFilePath.Of($"{localFeedPath.Value}\\{packageName}");
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



