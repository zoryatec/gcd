using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystem(
        IUploadAzBlobService _uploadService,
        IDownloadAzBlobService downloadAz,
        IFileSystem _fs,
        IWebDownload _webDownload,
        RemoteFileSystemSmb _remoteSmb
        ) : IRemoteFileSystem
    {

        public async Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
                WebUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
                AzBlobUri source => await downloadAz.DownloadFileAsync(source, destinationPath),
                SmbPath source => await _remoteSmb.DownloadFileAsync(source, destinationPath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }

        public async Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, sourcePath, overwrite: overwrite),
                AzBlobUri source => await _uploadService.UploadFileAsync(source, sourcePath),
                SmbPath source => await _remoteSmb.UploadFileAsync(source, sourcePath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }

        public Result<IFileDescriptor> CreateFileDescriptor(IDirectoryDescriptor dirDescriptor, FileName fileName)
        {
            return dirDescriptor switch
            {
                AzBlobContainerUri source => CreateBlobURI(source,fileName).Map((x) => x as IFileDescriptor),
                SmbDir source => CreateSmbFile(source, fileName).Map((x) => x as IFileDescriptor),
                _ => throw new InvalidOperationException(dirDescriptor.GetType().Name)
            };
        }

        private Result<SmbPath> CreateSmbFile(SmbDir azBlobContainerUri, FileName fileName)
        {


            return SmbPath.Of("");
        }

        private Result<AzBlobUri> CreateBlobURI(AzBlobContainerUri azBlobContainerUri, FileName fileName)
        {
            string nipkgUrl = CreateSubUrl(azBlobContainerUri, fileName.Value);

            return AzBlobUri.Create(nipkgUrl);
        }

        private string CreateSubUrl(AzBlobContainerUri feedUri, string subPath) =>
            $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";


    }
}
