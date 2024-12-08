using CSharpFunctionalExtensions;
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
        public async Task<Result> DownloadFileAsync(WebUri webUri, FilePath filePath)
        {
            using (WebClient client = new WebClient())
            {
                // Download the file asynchronously
                client.DownloadFile(webUri.Value, filePath.Value);
                Console.WriteLine($"File downloaded successfully to {filePath}");
            }
            return Result.Success();
        }
    }
}
