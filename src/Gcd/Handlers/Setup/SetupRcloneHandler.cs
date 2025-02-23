using System.Reflection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.Handlers.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Setup;
public record SetupRcloneRequest() : IRequest<UnitResult<Error>>;

public class SetupRcloneHandler(IMediator mediator, IFileSystem fileSystem)
    : IRequestHandler<SetupRcloneRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetupRcloneRequest request, CancellationToken cancellationToken)
    {
        var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var rcloneInstalationDir = LocalDirPath.Of($"{exeDirectory}\\rclone");
        
        await fileSystem.CreateDirAsync(rcloneInstalationDir.Value);
        
        var result =
            from instalationDirPath in rcloneInstalationDir
            from relativeContentDir in RelativeDirPath.Of("rclone-v1.69.1-windows-amd64")
            from archiveUri in WebFileUri
                .Of("https://github.com/rclone/rclone/releases/download/v1.69.1/rclone-v1.69.1-windows-amd64.zip")
                .MapError(er => new Error(er))
            select new DownloadArchiveRequest(archiveUri, relativeContentDir, instalationDirPath);

        await result
            .Bind(req => mediator.Send(req, cancellationToken));
        return result;
    }


}


