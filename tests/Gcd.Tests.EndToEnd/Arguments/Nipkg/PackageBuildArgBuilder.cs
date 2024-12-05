using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class PackageBuildArgBuilder : ArgumentsBuilder
    {
        public PackageBuildArgBuilder()
        {
            WithArg("nipkg");
            WithArg("package");
            WithArg("create");
        }

        public PackageBuildArgBuilder WithPackageContentDirectory(string packageContentDirectory)
        {
            WithOption("--package-sourec-dir", packageContentDirectory);
            return this;
        }
        public PackageBuildArgBuilder WithPackageName(string packageName)
        {
            WithOption("--package-name", packageName);
            return this;
        }

        public PackageBuildArgBuilder WithPackageVersion(string packageVersion)
        {
            WithOption("--package-version", packageVersion);
            return this;
        }

        public PackageBuildArgBuilder WithPackageInstalationDir(string packageInstalationDir)
        {
            WithOption("--package-instalation-dir", packageInstalationDir);
            return this;
        }

        public PackageBuildArgBuilder WithPackageDestinationDir(string packageInstalationDir)
        {
            WithOption("--package-destination-dir", packageInstalationDir);
            return this;
        }
    }
}