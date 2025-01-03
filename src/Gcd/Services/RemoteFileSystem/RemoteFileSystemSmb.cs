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
        public Result<IFileDescriptor> CreateFileDescriptor(SmbDirPath dirDescriptor, FileName fileName)
        {
            throw new NotImplementedException();
            //return Result.Success();
        }

        public async Task<Result> DownloadFileAsync(SmbDirPath smbDir, SmbFilePath sourceDescriptor, LocalFilePath destinationPath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false)
        {


  
                // Create a network credential
                var credentials = new NetworkCredential(SmbUserName.Value, SmbPassword.Value, null);

                // Use a network connection with SMB path
                using (new NetworkConnection(smbDir.Value, credentials))
                {
                // Read the file from SMB and write it to the local path

                File.Copy(sourceDescriptor.Value, destinationPath.Value, overwrite: true);
                    Console.WriteLine("File downloaded successfully!");
                }

            return Result.Success();

        }

        public async Task<Result> UploadFileAsync(SmbDirPath smbDir, SmbFilePath smbPath, LocalFilePath sourcePath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false)
        {
            UploadFile( smbDir.Value,smbPath.Value, sourcePath.Value, SmbUserName.Value, SmbPassword.Value);
            return Result.Success();
        }

        public static void UploadFile(string smbSharePath, string smbFilePath, string localFilePath, string username, string password, string domain = null)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(smbFilePath)) throw new ArgumentNullException(nameof(smbFilePath));
            if (string.IsNullOrWhiteSpace(localFilePath)) throw new ArgumentNullException(nameof(localFilePath));
            if (!File.Exists(localFilePath)) throw new FileNotFoundException("Local file does not exist", localFilePath);

            // Combine the file name with the SMB share path
            string fileName = Path.GetFileName(localFilePath);
            string destinationPath = Path.Combine(smbFilePath, fileName);

            // Create network credentials
            NetworkCredential credentials = new NetworkCredential(username, password, domain);

            // Connect to the SMB share and copy the file
            using (new NetworkConnection(smbSharePath, credentials))
            {
                File.Copy(localFilePath, smbFilePath, overwrite: true);
                Console.WriteLine($"File uploaded successfully to {destinationPath}");
            }
        }
    }
}
