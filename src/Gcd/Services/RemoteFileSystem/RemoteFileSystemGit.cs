using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemGit
    {
        public Result<IFileDescriptor> CreateFileDescriptor(IDirectoryDescriptor dirDescriptor, FileName fileName)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
        {
            throw new NotImplementedException();
        }
    }
}
