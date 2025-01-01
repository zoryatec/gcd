using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemSmb
    {
        public Result<IFileDescriptor> CreateFileDescriptor(SmbDir dirDescriptor, FileName fileName)
        {
            throw new NotImplementedException();
            //return Result.Success();
        }

        public async Task<Result> DownloadFileAsync(SmbPath sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
        {
            //throw new NotImplementedException();
            return Result.Success();


        }

        public async Task<Result> UploadFileAsync(SmbPath sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
        {
            //throw new NotImplementedException();
            return Result.Success();
        }
    }
}
