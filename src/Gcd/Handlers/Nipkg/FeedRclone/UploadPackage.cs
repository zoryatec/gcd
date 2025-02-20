using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.RemoteFileSystem.Rclone.Abstractions;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedRclone;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemRclone _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionRclone>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionRclone> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDefinition, packageFilePaths) = request;

        foreach (var packageFilePath in packageFilePaths)
        {
            var path = $"{remoteFeedDefinition.Feed.Value}/{packageFilePath.FileName.Value}";
            var packagePath = RcloneFilePath.Of(path);
             var result = await _rfs.UploadFileAsync(packagePath.Value,packageFilePath);
            if (result.IsFailure) return result.MapError(er => er.Message);
        }
        return  Result.Success();
    }
}
