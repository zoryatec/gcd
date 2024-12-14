using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

public interface IDownloadAzBlobService
{
    public Task<Result> DownloadFileAsync(AzBlobUri blobUri, LocalFilePath filePath);
}

