using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;

namespace Gcd.Services.RemoteFileSystem
{
    public interface IRemoteFileSystemSmb
    {
        public Task<Result> DownloadFileAsync(SmbDirPath smbDir, SmbFilePath sourceDescriptor, LocalFilePath destinationPath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false);

        public Task<Result> UploadFileAsync(SmbDirPath smbDir, SmbFilePath smbPath, LocalFilePath sourcePath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false);

    }
}
