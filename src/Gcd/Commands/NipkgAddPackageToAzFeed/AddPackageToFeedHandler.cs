
using System.Diagnostics;
using Azure.Core;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Common;
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

 public record AddPackageToFeedRequest(FeedUri FeedUri, PackagePath PackagePath) : IRequest<UnitResult<Error>>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(
    IMediator mediator,
    IUploadAzBlobService uploadService)
    : IRequestHandler<AddPackageToFeedRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string currentDirectoryPath = Environment.CurrentDirectory;


        var localFeedPath = temporaryDirectory;
        var localFeedPath1 = FeedPath.Create(localFeedPath);
        var downloadReq = new NipkgPullFeedMetaRequest(request.FeedUri, localFeedPath1.Value);
        string packageName = Path.GetFileName(request.PackagePath.Value);
        var packageDestinationPath = Path.Combine(localFeedPath, packageName);

        var downloadResult = await mediator.Send(downloadReq);

        File.Copy(request.PackagePath.Value, packageDestinationPath, true);

        var addPakcgResult = await AddPackageToLcalFeed(localFeedPath, packageDestinationPath);

        var pushRequest = new NipkgPushAzBlobFeedMetaRequest(request.FeedUri, FeedPath.Create(localFeedPath).Value);
        var pushResult = await mediator.Send(pushRequest);


        if (pushResult.IsFailure) return pushResult;

        //Directory.Delete(temporaryDirectory, true);


            
        return await UploadPackage(request.FeedUri, FeedPath.Create(localFeedPath).Value, packageName);
    }

    private async Task<UnitResult<Error>> UploadPackage(FeedUri feedUri, FeedPath localFeedPath, string packageName)
    {
        string nipkgUrl = CreateSubUrl(feedUri, packageName);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var filePath = FilePath.Create($"{localFeedPath.Value}\\{packageName}");
        var result = await uploadService.UploadFileAsync(blobUri.Value, filePath.Value);
        return result;
    }

    private string CreateSubUrl(FeedUri feedUri, string subPath)
    {
        return $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";
    }

    private async Task<UnitResult<Error>> AddPackageToLcalFeed(string feedDir, string packagePath)
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



