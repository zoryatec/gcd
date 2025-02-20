using System.Diagnostics;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.RemoteFileSystem.Rclone.Abstractions;

namespace Gcd.RemoteFileSystem.Rclone;

public class RemoteFileSystemRclone : IRemoteFileSystemRclone
{
    public async Task<Result> DownloadFileAsync(RcloneFilePath sourceFilePath, ILocalFilePath destinationFilePath,
        bool overwrite = false)
    {
        return RunRcloneCli("copyto",sourceFilePath.Value, destinationFilePath.Value);
    }

    public async Task<UnitResult<Error>> UploadFileAsync(RcloneFilePath destinationFilePath, ILocalFilePath sourceFilePath,
        bool overwrite = false)
    {
        return RunRcloneCli("copyto",sourceFilePath.Value, destinationFilePath.Value).MapError(x => new Error(x));
    }
    
    private Result RunRcloneCli( params string[] args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "rclone",  
            RedirectStandardOutput = true, 
            RedirectStandardError = true,  
            UseShellExecute = false,    
            CreateNoWindow = true
        };
        foreach (var arg in args)
        {
            startInfo.ArgumentList.Add(arg);
        }

        try
        {
            using (Process? process = Process.Start(startInfo))
            {
                _ = process ?? throw new ArgumentNullException(nameof(process));
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                
                if (!string.IsNullOrEmpty(errors))  return Result.Failure<string>(errors);
                return Result.Success(output);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

}