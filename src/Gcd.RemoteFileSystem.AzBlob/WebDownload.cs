using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services
{
    public class WebDownload : IWebDownload
    {
        public async Task<Result> DownloadFileAsync(IWebFileUri webFileUri, ILocalFilePath filePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                using (var responseStream = await httpClient.GetStreamAsync(webFileUri.Value))
                using (var fileStream = new FileStream(filePath.Value, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await responseStream.CopyToAsync(fileStream);
                }
                return Result.Success();
            }
            catch (Exception ex) 
            {
                return Result.Failure(ex.Message);
            }

        }

        public Task<Result> DownloadFileAsync(IWebFileUri webFileUri, ILocalDirPath directoryPath)
        {
            // var localFilePath =  LocalFilePath.Of(Lo)
            throw new NotImplementedException();
        }
    }
}
