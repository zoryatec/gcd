using CSharpFunctionalExtensions;
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
        var (feedDef, packageFilePath) = request;

        var smbFilePath = SmbFilePath.Of($"{feedDef.Feed.Value}\\{packageFilePath.FileName.Value}");

        return await smbFilePath
            .Bind((smbPath) => _rfs.UploadFileAsync(feedDef.Feed, smbPath, packageFilePath, feedDef.SmbUserName, feedDef.SmbPassword));
    }
}
