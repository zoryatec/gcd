

using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;


namespace Gcd.RemoteFileSystem.Rclone.Abstractions;

public interface IRemoteFileSystemRclone
{
    public Task<Result> DownloadFileAsync(RcloneFilePath sourceFilePath, ILocalFilePath destinationFilePath, bool overwrite = false);

    public Task<UnitResult<Error>> UploadFileAsync( RcloneFilePath destinationFilePath, ILocalFilePath sourceFilePath, bool overwrite = false);

}

