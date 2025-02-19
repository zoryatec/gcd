using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.PackageBuilder
{
    public record ContentLink(InatallationTargetRootDir TargetRootDir, PackageBuilderContentSourceDir ContentSourceDir)
    {
        public static ContentLink Of(InatallationTargetRootDir TargetRootDir, PackageBuilderContentSourceDir ContentSourceDir) => new ContentLink(TargetRootDir, ContentSourceDir);
    }
}
