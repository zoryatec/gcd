using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcd.Common;

namespace Gcd.Services.RemoteFileSystem
{
    public interface IRemoteFileSystemGit
    {
        public Task<Result> Clone(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutDir);
        public Task<Result> Push(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutDir);
        
        public Task<UnitResult<Error>> DownloadFileAsync(IRelativeFilePath sourcePath, ILocalFilePath destinationPath, GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password);
        public Task<UnitResult<Error>> UploadFileAsync(IFileDescriptor sourcePath, IRelativeFilePath destinationPath,  GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password);
    }
}
