using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGitHub;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemGit rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePaths) = request;
        
        foreach (var packageFilePath in packageFilePaths)
        {
            var result = await rfs.UploadFileAsync(packageFilePath,
                new RelativeFilePath(RelativeDirPath.Root, packageFilePath.FileName), feedDef.Address,
                feedDef.BrancName, feedDef.UserName, feedDef.Password, feedDef.CommitterName, feedDef.CommitterEmail);
            
            if (result.IsFailure) return result.MapError(x => x.Message);
        }
        return  Result.Success();
    }

}
