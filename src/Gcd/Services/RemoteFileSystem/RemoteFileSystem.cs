using CSharpFunctionalExtensions;
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
        IWebDownload _webDownload
        ) : IRemoteFileSystem
    {

        public async Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
                WebUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
                AzBlobUri source => await downloadAz.DownloadFileAsync(source, destinationPath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }

        public async Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
        {
            return sourceDescriptor switch
            {
                LocalFilePath source => await _fs.CopyFileAsync(source, sourcePath, overwrite: overwrite),
                AzBlobUri source => await _uploadService.UploadFileAsync(source, sourcePath),
                _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
            };
        }
    }
}
