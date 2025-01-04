using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.RemoteFileSystem
{
    public interface IRemoteFileSystemAzBlob
    {
        public Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false);
        public Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false);
        public Result<IFileDescriptor> CreateFileDescriptor(IDirectoryDescriptor dirDescriptor, FileName fileName);
    }
}
