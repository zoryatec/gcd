using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedAzBlob;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemAzBlob _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePaths) = request;

        foreach (var packageFilePath in packageFilePaths)
        {
            var result = await UploadPackagePriv(feedDef.Feed, packageFilePath);
            if (result.IsFailure) return result;
        }
        return  Result.Success();
    }

    private async Task<Result> UploadPackagePriv(IDirectoryDescriptor dirDescriptor, PackageLocalFilePath packagePath)
    {

        var blorUriRes = _rfs.CreateFileDescriptor(dirDescriptor, packagePath.FileName);

        var result = await _rfs.UploadFileAsync(blorUriRes.Value, packagePath);
        return result;
    }
}
