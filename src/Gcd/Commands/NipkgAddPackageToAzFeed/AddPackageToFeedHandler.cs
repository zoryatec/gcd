using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Services;
using MediatR;

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

 public record AddPackageToFeedRequest(AzBlobFeedUri FeedUri, PackagePath PackagePath) : IRequest<Result>;
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
        var downloadReq = new NipkgPullFeedMetaRequest(AzBlobFeedDefinition.Of(request.FeedUri).Value, localFeedDef.Value);
        string packageName = Path.GetFileName(request.PackagePath.Value);
        var packageDestinationPath = Path.Combine(localFeedPath, packageName);

        var downloadResult = await mediator.Send(downloadReq);

        File.Copy(request.PackagePath.Value, packageDestinationPath, true);

        var addPakcgResult = await AddPackageToLcalFeed(localFeedPath, packageDestinationPath);

        var azFeedDef = AzBlobFeedUri.Create(request.FeedUri.Full)
            .Bind(feedUri => AzBlobFeedDefinition.Of(feedUri));

        var localFeedDef3 = LocalDirPath.Of(localFeedPath)
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));

        var pushRequest = new NipkgPushAzBlobFeedMetaRequest(azFeedDef.Value, localFeedDef3.Value);
        var pushResult = await mediator.Send(pushRequest);


        if (pushResult.IsFailure) return pushResult;

        //Directory.Delete(temporaryDirectory, true);


            
        return await UploadPackage(request.FeedUri, FeedPath.Create(localFeedPath).Value, packageName);
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
        // Initialize the ProcessStartInfo with the command
        //string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";
        //string arguments = $"/c {nipkg} feed-add-pkg {feedDir} {packagePath}";


        //ProcessStartInfo startInfo = new ProcessStartInfo
        //{
        //    FileName = "cmd.exe",       // Use "cmd.exe" to run a command
        //    Arguments = arguments, // "/c" tells cmd to run the command and then terminate
        //    RedirectStandardOutput = true, // Redirect the output of the command
        //    RedirectStandardError = true,  // Redirect any errors
        //    UseShellExecute = false,      // Don't use the shell to execute the command
        //    CreateNoWindow = true        // Don't create a command window
        //};

        //try
        //{
        //    using (Process process = Process.Start(startInfo))
        //    {
        //        // Read the standard output and error
        //        string output = process.StandardOutput.ReadToEnd();
        //        string errors = process.StandardError.ReadToEnd();

        //        // Wait for the command to finish
        //        process.WaitForExit();

        //        // Print the output and errors (if any)
        //        Console.WriteLine("Output:");
        //        Console.WriteLine(output);

        //        if (!string.IsNullOrEmpty(errors))
        //        {
        //            Console.WriteLine("Errors:");
        //            Console.WriteLine(errors);
        //        }

        //    }
        //    return UnitResult.Success<Error>();
        //}
        //catch (Exception ex)
        //{
        //     return UnitResult.Failure<Error>(new Error($"Error running command: {ex.Message}"));
        //}
    }

}



