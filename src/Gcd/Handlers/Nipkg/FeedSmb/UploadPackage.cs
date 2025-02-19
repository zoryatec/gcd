using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedSmb;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemSmb _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePaths) = request;
        foreach (var packageFilePath in packageFilePaths)
        {
            var smbFilePath = SmbFilePath.Of($"{feedDef.Feed.Value}\\{packageFilePath.FileName.Value}");
            
            var result = await smbFilePath
                .Bind((smbPath) => _rfs.UploadFileAsync(feedDef.Feed, smbPath, packageFilePath, feedDef.SmbUserName, feedDef.SmbPassword))
                .MapError(err => err.Message);
            
            if (result.IsFailure) return result;
        }
        return  Result.Success();
    }
}
