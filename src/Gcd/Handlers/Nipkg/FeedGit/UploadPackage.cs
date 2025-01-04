using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.File;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class UploadPackage(IFileSystem _fs, RemoteFileSystemGit _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePath) = request;


        var checkoutFeed = FeedDefinitionLocal.Of(_rfs.GlobalCheckoutDir);

        var destinationPackage = checkoutFeed
            .Map((arg) => PackageFilePath.Of(arg.Feed, packageFilePath.FileName));

        return await destinationPackage
        .Bind((x) => _fs.CopyFileAsync(packageFilePath, destinationPackage.Value, overwrite: true))
        .Bind(() => _rfs.Push(feedDef.Address, feedDef.BrancName, feedDef.UserName, feedDef.Password, feedDef.CommitterName, feedDef.CommitterEmail));
    }

}
