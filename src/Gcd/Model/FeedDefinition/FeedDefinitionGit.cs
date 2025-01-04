using Gcd.Model.File;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.FeedDefinition;

public record FeedDefinitionGit(GitRepoAddress Address, GitLocalBranch BrancName, GitUserName UserName, GitPassword Password, GitCommitterName CommitterName, GitCommiterEmail CommitterEmail) : IFeedDefinition
{
    public IDirectoryDescriptor Feed => throw new NotImplementedException();

    public IFileDescriptor Package => throw new NotImplementedException();

    public IFileDescriptor PackageGz => throw new NotImplementedException();

    public IFileDescriptor PackageStamps => throw new NotImplementedException();
}
