using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model;
using Gcd.Services;
using Gcd.Services.RemoteFileSystem;

namespace Gcd.RemoteFileSystem.AzBlob
{
    public class RemoteFileSystemAzBlob(
        IUploadAzBlobService _uploadService,
        IDownloadAzBlobService downloadAz,
        IFileSystem _fs,
        IWebDownload _webDownload
        ) : IRemoteFileSystemAzBlob
    {

        public async Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, ILocalFilePath destinationPath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
                WebUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
                AzBlobUri source => await downloadAz.DownloadFileAsync(source, destinationPath),
                //SmbFilePath source => await _remoteSmb.DownloadFileAsync(source, destinationPath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }

        public async Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, ILocalFilePath sourcePath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, sourcePath, overwrite: overwrite),
                AzBlobUri source => await _uploadService.UploadFileAsync(source, sourcePath),
                //SmbPath source => await _remoteSmb.UploadFileAsync(source, sourcePath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }



        private Result<AzBlobUri> CreateBlobURI(AzBlobContainerUri azBlobContainerUri, IFileName fileName)
        {
            string nipkgUrl = CreateSubUrl(azBlobContainerUri, fileName.Value);

            return AzBlobUri.Create(nipkgUrl);
        }

        private string CreateSubUrl(AzBlobContainerUri feedUri, string subPath) =>
            $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";


        public Result<IFileDescriptor> CreateFileDescriptor(IDirectoryDescriptor dirDescriptor, IFileName fileName)
        {
            return dirDescriptor switch
            {
                AzBlobContainerUri source => CreateBlobURI(source, fileName).Map((x) => x as IFileDescriptor),
                _ => throw new InvalidOperationException(dirDescriptor.GetType().Name)
            };
        }
    }
}
