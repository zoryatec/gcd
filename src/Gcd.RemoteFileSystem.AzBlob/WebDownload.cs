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
        public async Task<Result> DownloadFileAsync(WebUri webUri, ILocalFilePath filePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                using (var responseStream = await httpClient.GetStreamAsync(webUri.Value))
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
    }
}
