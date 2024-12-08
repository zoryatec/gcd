using CSharpFunctionalExtensions;
using Gcd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

public interface IDownloadAzBlobService
{
    public Task<Result> DownloadFileAsync(AzBlobUri blobUri, FilePath filePath);
}

