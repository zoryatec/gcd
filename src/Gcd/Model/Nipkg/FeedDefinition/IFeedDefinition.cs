using Gcd.LocalFileSystem.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.FeedDefinition
{
    public interface IFeedDefinition
    {
        public IDirectoryDescriptor Feed { get; }
        public IFileDescriptor Package { get; }
        public IFileDescriptor PackageGz { get; }
        public IFileDescriptor PackageStamps { get; }
    }
}
