using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;

namespace Gcd.Services.RemoteFileSystem
{
    public interface IRemoteFileSystemSmb
    {
        public Task<Result> DownloadFileAsync(SmbDirPath smbDir, SmbFilePath sourceDescriptor, ILocalFilePath destinationPath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false);

        public Task<UnitResult<Error>> UploadFileAsync(SmbDirPath smbDir, SmbFilePath smbPath, ILocalFilePath sourcePath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false);

    }
}
