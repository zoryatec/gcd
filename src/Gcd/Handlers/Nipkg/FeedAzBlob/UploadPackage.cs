using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedAzBlob;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemAzBlob _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionAzBlob>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionAzBlob> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePath) = request;
        return await UploadPackagePriv(feedDef.Feed, packageFilePath);
    }

    private async Task<Result> UploadPackagePriv(IDirectoryDescriptor dirDescriptor, PackageFilePath packagePath)
    {

        var blorUriRes = _rfs.CreateFileDescriptor(dirDescriptor, packagePath.FileName);

        var result = await _rfs.UploadFileAsync(blorUriRes.Value, packagePath);
        return result;
    }
}
