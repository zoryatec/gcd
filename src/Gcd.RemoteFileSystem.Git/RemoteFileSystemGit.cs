using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using LibGit2Sharp;


namespace Gcd.Services.RemoteFileSystem
{
    public class RemoteFileSystemGit : IRemoteFileSystemGit
    {

        public readonly LocalDirPath GlobalCheckoutDir;
        private bool _allready_cloned = false;

        public async Task<Result> Clone(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutDir)
        {
            //if(!_allready_cloned) return Result.Try(() => CloneRepositoryWithCredentials(address, branch, username, password, checkoutDir), ex => ex.Message).MapError(errr => $"PULL:{errr}");
            //_allready_cloned = true;
            //return Result.Success();
            return Result.Try(() => CloneRepositoryWithCredentials(address, branch, username, password, checkoutDir), ex => ex.Message).MapError(errr => $"PULL:{errr}");
        }

        public async Task<Result> Push(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutDir)
        {
            return Result.Try(() => CommitAndPush(address, branch, username, password, committerName, committerEmail, checkoutDir)).MapError(errr => $"PUSH:{errr}");
        }


        public void CloneRepositoryWithCredentials(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password, LocalDirPath checkoutPath)
        {
            var fetchOptions = new FetchOptions
            {
                CredentialsProvider = (url, user, cred) =>
                    new UsernamePasswordCredentials
                    {
                        Username = username.Value,
                        Password = password.Value
                    }
            };

            var cloneOptions = new CloneOptions(fetchOptions)
            {
                BranchName = branch.Value
            };

            Repository.Clone(address.Value, checkoutPath.Value, cloneOptions);

        }


        public void CommitAndPush(GitRepoAddress address, GitLocalBranch branch, GitUserName username, GitPassword password,GitCommitterName committerName, GitCommiterEmail committerEmail, LocalDirPath checkoutPath)
        {
            using (var repo = new Repository(checkoutPath.Value))
            {
                LibGit2Sharp.Commands.Stage(repo, "*");

                var status = repo.RetrieveStatus();
                if (status.IsDirty)
                {
                    var author = new Signature(committerName.Value, committerEmail.Value, DateTime.Now);
                    var committer = author;
                    repo.Commit("gcd-commit", author, committer);
                }
                else
                {
                    throw new Exception("No changes to commit."); // replace with result
                }

                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials
                        {
                            Username = username.Value,
                            Password = password.Value
                        }
                };

                repo.Network.Push(repo.Branches[branch.Value], pushOptions);
            }
        }
    }
}
