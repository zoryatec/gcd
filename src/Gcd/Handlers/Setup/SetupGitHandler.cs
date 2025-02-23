using System.Reflection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Setup;
public record SetupGitRequest() : IRequest<UnitResult<Error>>;

public class SetupGitHandler(IMediator mediator, IFileSystem fileSystem)
    : IRequestHandler<SetupGitRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetupGitRequest request, CancellationToken cancellationToken)
    {
        var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var gitInstalationDir = LocalDirPath.Of($"{exeDirectory}\\git");
        
        await fileSystem.CreateDirAsync(gitInstalationDir.Value);

        var result =
            from instalationDirPath in gitInstalationDir
            from archiveUri in WebFileUri
                .Of("https://github.com/git-for-windows/git/releases/download/v2.48.1.windows.1/MinGit-2.48.1-64-bit.zip")
                .MapError(er => new Error(er))
            select new DownloadArchiveRequest(archiveUri, RelativeDirPath.None, instalationDirPath);

        await result
            .Bind(req => mediator.Send(req, cancellationToken));
        return result;
    }


}


