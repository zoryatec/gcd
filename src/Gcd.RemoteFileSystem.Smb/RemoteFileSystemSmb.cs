
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using System.Net;


namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemSmb : IRemoteFileSystemSmb
    {
        public Result<IFileDescriptor> CreateFileDescriptor(SmbDirPath dirDescriptor, FileName fileName)
        {
            throw new NotImplementedException();
            //return Result.Success();
        }

        public async Task<Result> DownloadFileAsync(SmbDirPath smbDir, SmbFilePath sourceDescriptor, ILocalFilePath destinationPath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false) =>
            await Result.Try(() => DownloadFilePrivAsync(smbDir, sourceDescriptor, destinationPath, SmbUserName, SmbPassword, overwrite), ex => ex.Message);


        private async Task<Result> DownloadFilePrivAsync(SmbDirPath smbDir, SmbFilePath sourceDescriptor, ILocalFilePath destinationPath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false)
        {
            var credentials = new NetworkCredential(SmbUserName.Value, SmbPassword.Value, null);

            using (new NetworkConnection(smbDir.Value, credentials))
            {
                File.Copy(sourceDescriptor.Value, destinationPath.Value, overwrite: true);
            }

            return Result.Success();

        }

        public async Task<UnitResult<Error>> UploadFileAsync(SmbDirPath smbDir, SmbFilePath smbPath, ILocalFilePath sourcePath, SmbUserName SmbUserName, SmbPassword SmbPassword, bool overwrite = false) =>
            Result.Try(() => UploadFile(smbDir.Value, smbPath.Value, sourcePath.Value, SmbUserName.Value, SmbPassword.Value), ex => new Error(ex.Message));

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
            }
        }
    }
}
