using System.Diagnostics;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using LibGit2Sharp;


namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemGit(IFileSystem _fs) : IRemoteFileSystemGit
    {

        public readonly LocalDirPath GlobalCheckoutDir;
        private bool _allready_cloned = false;

        public async Task<Result> Clone(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutDir)
        {
            //if(!_allready_cloned) return Result.Try(() => CloneRepositoryWithCredentials(address, branch, username, password, checkoutDir), ex => ex.Message).MapError(errr => $"PULL:{errr}");
            //_allready_cloned = true;
            //return Result.Success();
            return CloneCmd(address, branch, username, password, checkoutDir).MapError(err => err.Message);
            // return Result.Try(() => CloneRepositoryWithCredentials(address, branch, username, password, checkoutDir), ex => ex.Message).MapError(errr => $"PULL:{errr}");
        }

        public async Task<Result> Push(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutDir)
        {
            return GitCommitAndPushCmds(address, username, password, committerName, committerEmail, checkoutDir).MapError(x => x.Message);
            // return Result.Try(() => CommitAndPush(address, branch, username, password, committerName, committerEmail, checkoutDir)).MapError(errr => $"PUSH:{errr}");
        }

        public  async Task<UnitResult<Error>> DownloadFileAsync(IRelativeFilePath sourcePath, ILocalFilePath destinationPath, GitRepoAddress address,
            GitLocalBranch branch, GitUserName username, GitPassword password)
        {
            var tempDirResult = await _fs.GenerateTempDirectoryAsync();
            var tempDir = tempDirResult.Value; 
            
            var result  = CloneNoCheckoutCmd(address, username, password, tempDir)
                .Bind(() => GitSparseCheckoutInit(tempDir))
                .Bind(() =>  GitSparseCheckoutSet(tempDir, sourcePath))
                .Bind(() => GitCheckout(tempDir,branch));
            if (result.IsFailure) return result;
           
            string path = tempDirResult.Value +"\\" + sourcePath.Value;
            var tempFileResult = LocalFilePath.Of(path);
            var result1 = await _fs.CopyFileAsync(tempFileResult.Value, destinationPath);
            
            return result1.MapError(x => new Error(x));
        }

        public async Task<UnitResult<Error>> UploadFileAsync(ILocalFilePath sourcePath, IRelativeFilePath destinationPath, GitRepoAddress address,
            GitLocalBranch branch, GitUserName username, GitPassword password,GitCommitterName committerName, GitCommiterEmail committerEmail)
        {
            var tempDirResult = await _fs.GenerateTempDirectoryAsync();
            var tempDir = tempDirResult.Value; 
            
            var result = CloneNoCheckoutCmd(address, username, password, tempDir)
                .Bind(() => GitSparseCheckoutInit(tempDir))
                .Bind(() =>  GitSparseCheckoutSet(tempDir, destinationPath))
                .Bind(() => GitCheckout(tempDir,branch));
            
            if (result.IsFailure) return result;
            
            string path = tempDirResult.Value +"\\" + destinationPath.Value;
            var tempFileResult = LocalFilePath.Of(path);
            var result1 = await _fs.CopyFileAsync(sourcePath, tempFileResult.Value, overwrite: true);
            if (result1.IsFailure) return result;

            
            return RunGitCmd( "-C", tempDir.Value, "config","user.email",committerEmail.Value)
                .Bind( () => RunGitCmd("-C", tempDir.Value, "config","user.name",committerName.Value))
                .Bind(() => GitAddCmd(tempDir))
                .Bind( () => GitCommitCmd(committerName, committerEmail, tempDir))
                .Bind( () => GitPushCmd(address, username, password, tempDir));
        }


        // public void CloneRepositoryWithCredentials(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutPath)
        // {
        //     var fetchOptions = new FetchOptions
        //     {
        //         CredentialsProvider = (url, user, cred) =>
        //             new UsernamePasswordCredentials
        //             {
        //                 Username = username.Value,
        //                 Password = password.Value
        //             }
        //     };
        //
        //     var cloneOptions = new CloneOptions(fetchOptions)
        //     {
        //         BranchName = branch.Value
        //     };
        //
        //     Repository.Clone(address.Value, checkoutPath.Value, cloneOptions);
        //
        // }


        // public void CommitAndPush(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password,GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutPath)
        // {
        //     using (var repo = new Repository(checkoutPath.Value))
        //     {
        //         LibGit2Sharp.Commands.Stage(repo, "*");
        //
        //         var status = repo.RetrieveStatus();
        //         if (status.IsDirty)
        //         {
        //             var author = new Signature(committerName.Value, committerEmail.Value, DateTime.Now);
        //             var committer = author;
        //             repo.Commit("gcd-commit", author, committer);
        //         }
        //         else
        //         {
        //             throw new Exception("No changes to commit."); // replace with result
        //         }
        //
        //         var pushOptions = new PushOptions
        //         {
        //             CredentialsProvider = (url, usernameFromUrl, types) =>
        //                 new UsernamePasswordCredentials
        //                 {
        //                     Username = username.Value,
        //                     Password = password.Value
        //                 }
        //         };
        //
        //         repo.Network.Push(repo.Branches[branch.Value], pushOptions);
        //     }
        // }
        
        private UnitResult<Error> CloneNoCheckoutCmd(GitRepoAddress address, GitUserName username, GitPassword password, LocalDirPath checkoutDir)
        {
            string url = address.Value;
            Uri uri = new Uri(url);
        
            // Extract host and absolute path
            string noHtttps = uri.Host + uri.AbsolutePath;
            string fullAddress = $"https://{username.Value}:{password.Value}@{noHtttps}";

            return RunGitCmd(
                "clone",
                "--no-checkout",
                fullAddress,
                checkoutDir.Value);
        }


        private UnitResult<Error>CloneCmd(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutDir)
        {
            string url = address.Value;
            Uri uri = new Uri(url);
        
            // Extract host and absolute path
            string noHtttps = uri.Host + uri.AbsolutePath;
            string fullAddress = $"https://{username.Value}:{password.Value}@{noHtttps}";

            return RunGitCmd(
                "clone",
                "--branch", branch.Value,
                fullAddress,
                checkoutDir.Value);
        }
        
        private UnitResult<Error> GitSparseCheckoutInit( LocalDirPath checkoutDir)
        {
            return RunGitCmd(
                "-C", checkoutDir.Value,
                "sparse-checkout",
                "init",
                "--no-cone"
                );
        }
        
        private UnitResult<Error> GitCheckout( LocalDirPath checkoutDir, GitLocalBranch branch)
        {
            return RunGitCmd(
                "-C", checkoutDir.Value,
                "checkout", branch.Value
            );
        }
        
        private UnitResult<Error> GitSparseCheckoutSet(LocalDirPath checkoutDir, IRelativeFilePath pathToCheckout) =>
            GitSparseCheckoutSet(checkoutDir, [pathToCheckout]);
        private UnitResult<Error> GitSparseCheckoutSet(LocalDirPath checkoutDir, IReadOnlyList<IRelativeFilePath> pathsToCheckout)
        {
            var paths = pathsToCheckout.Select(x => x.Value).ToArray();

            var args = new List<string>()
            {
                "-C", checkoutDir.Value,
                "sparse-checkout",
                "set",
            };
            
            args.AddRange(paths);
            
            return RunGitCmd(args.ToArray());
        }
        
        
        private UnitResult<Error> GitCommitAndPushCmds(GitRepoAddress address, GitUserName username, GitPassword password,GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutDir)
        {
            return RunGitCmd( "-C", checkoutDir.Value, "config","user.email",committerEmail.Value)
                .Bind( () => RunGitCmd("-C", checkoutDir.Value, "config","user.name",committerName.Value))
                .Bind(() => GitAddCmd(checkoutDir))
                .Bind( () => GitCommitCmd(committerName, committerEmail, checkoutDir))
                .Bind( () => GitPushCmd(address, username, password, checkoutDir));
        }
        
        private UnitResult<Error>GitPushCmd(GitRepoAddress address, GitUserName username, GitPassword password, LocalDirPath checkoutDir)
        {
            string url = address.Value;
            Uri uri = new Uri(url);
        
            // Extract host and absolute path
            string noHtttps = uri.Host + uri.AbsolutePath;
            string fullAddress = $"https://{username.Value}:{password.Value}@{noHtttps}";

            return RunGitCmd(
                "-C", checkoutDir.Value,
                "push",
                fullAddress);
        }
        
        private UnitResult<Error> GitAddCmd( LocalDirPath checkoutDir)
        {
            return RunGitCmd(
                "-C", checkoutDir.Value,
                "add", "-A");
        }
        
        private UnitResult<Error>GitCommitCmd(GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutDir)
        {
            return RunGitCmd(
                "-C", checkoutDir.Value,
                "commit",
                "-m", "package added message",
                $"--author={committerName.Value} <{committerEmail.Value}>");
        }
        
        private UnitResult<Error> RunGitCmd(params string[] args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "git",  
                RedirectStandardOutput = true, 
                RedirectStandardError = true,  
                UseShellExecute = false,     
                CreateNoWindow = false 
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
                    process.WaitForExit(60 * 1000);

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    
                    if (process.ExitCode !=0)
                    {
                        return Result.Failure<string,Error>(new Error(errors));
                    }
                    return Result.Success<string,Error>(output);
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<string,Error>(new Error(ex.Message));
            }
        }
    }
}
