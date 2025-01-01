using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePath) = request;
        return  Result.Success();
    }

}
