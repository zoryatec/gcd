using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemGit rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePaths) = request;
        
        // var checkoutDir = await _fs.GenerateTempDirectoryAsync();
        // var checkoutFeed = FeedDefinitionLocal.Of(checkoutDir.Value);
        //
        // var pullResult = await checkoutFeed
        //     .Bind((x) => rfs.Clone(feedDef.Address, feedDef.BrancName, feedDef.UserName, feedDef.Password, checkoutFeed.Value.Feed));
        
        foreach (var packageFilePath in packageFilePaths)
        {
            var result = await rfs.UploadFileAsync(packageFilePath,
                new RelativeFilePath(RelativeDirPath.Root, packageFilePath.FileName), feedDef.Address,
                feedDef.BrancName, feedDef.UserName, feedDef.Password, feedDef.CommitterName, feedDef.CommitterEmail);
            // var destinationPackage = checkoutFeed
            //     .Map((arg) => PackageLocalFilePath.Of(arg.Feed, packageFilePath.FileName));
            //
            // var result = await destinationPackage
            //     .Bind((x) => _fs.CopyFileAsync(packageFilePath, destinationPackage.Value, overwrite: true))
            //     .Bind(() => _rfs.Push(feedDef.Address, feedDef.BrancName, feedDef.UserName, feedDef.Password, feedDef.CommitterName, feedDef.CommitterEmail, checkoutFeed.Value.Feed));
            
            if (result.IsFailure) return result.MapError(x => x.Message);
        }
        return  Result.Success();
    }

}
