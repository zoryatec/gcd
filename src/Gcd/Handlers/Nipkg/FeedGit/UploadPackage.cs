using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemGit _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePath) = request;


        var checkoutDir = await _fs.GenerateTempDirectoryAsync();
        var checkoutFeed = FeedDefinitionLocal.Of(checkoutDir.Value);

        var destinationPackage = checkoutFeed
            .Map((arg) => PackageLocalFilePath.Of(arg.Feed, packageFilePath.FileName));

        var pullResult = await checkoutFeed
            .Bind((x) => _rfs.Clone(feedDef.Address, feedDef.BrancName, feedDef.UserName, feedDef.Password, checkoutFeed.Value.Feed));

        return await destinationPackage
        .Bind((x) => _fs.CopyFileAsync(packageFilePath, destinationPackage.Value, overwrite: true))
        .Bind(() => _rfs.Push(feedDef.Address, feedDef.BrancName, feedDef.UserName, feedDef.Password, feedDef.CommitterName, feedDef.CommitterEmail, checkoutFeed.Value.Feed));
    }

}
