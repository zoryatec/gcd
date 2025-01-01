using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using SMBLibrary;
using SMBLibrary.Client;
using SMBLibrary.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;

namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemSmb : IRemoteFileSystemSmb
    {
        public Result<IFileDescriptor> CreateFileDescriptor(SmbDir dirDescriptor, FileName fileName)
        {
            throw new NotImplementedException();
            //return Result.Success();
        }

        public async Task<Result> DownloadFileAsync(SmbPath sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
        {
            // Azure File Share details
            //string connectionString = "your-azure-storage-connection-string"; // Azure Storage connection string
            //string shareName = "your-file-share"; // Your file share name
            //string fileName = "your-file.txt"; // File you want to download from Azure File Share
            //string downloadFilePath = @"C:\path\to\local\downloaded\file.txt"; // Local path to save the downloaded file

            //// Create a ShareServiceClient
            //StorageSharedKeyCredential azureKeyCredential = new StorageSharedKeyCredential(fileName, shareName);
            //var uri = new Uri("");
            //var shareClient = new ShareClient(uri, azureKeyCredential);

            //try
            //{
            //    // Get the file client for the specific file
            //    var fileClient = shareClient.GetDirectoryClient("").GetFileClient(fileName);

            //    // Check if the file exists
            //    if (await fileClient.ExistsAsync())
            //    {
            //        Console.WriteLine($"Starting download of {fileName}...");

            //        // Download the file
            //        ShareFileDownloadInfo download = await fileClient.DownloadAsync();

            //        // Open a local file stream to save the downloaded content
            //        using (FileStream fs = File.OpenWrite(downloadFilePath))
            //        {
            //            await download.Content.CopyToAsync(fs);
            //            Console.WriteLine($"File downloaded successfully to {downloadFilePath}");
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine($"File {fileName} does not exist in the share.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}

            return Result.Success();

        }

        public async Task<Result> UploadFileAsync(SmbPath sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
        {
            //throw new NotImplementedException();
            return Result.Success();
        }
    }
}
