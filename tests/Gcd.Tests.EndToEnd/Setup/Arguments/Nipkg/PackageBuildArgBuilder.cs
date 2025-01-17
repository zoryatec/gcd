﻿using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Build;
using Gcd.Commands.Nipkg.Builder;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuildArgBuilder : ArgumentsBuilder
{
    public PackageBuildArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseCmdBuildExtension.NAME);
    }

    public PackageBuildArgBuilder WithPackageContentDirectory(string packageContentDirectory)
    {
        WithOption(BuilderContentSourceDirOption.NAME, packageContentDirectory);
        return this;
    }
    public PackageBuildArgBuilder WithPackageName(string packageName)
    {
        WithOption(PackageNameOption.NAME, packageName);
        return this;
    }

    public PackageBuildArgBuilder WithPackageVersion(string packageVersion)
    {
        WithOption(PackageVersionOption.NAME, packageVersion);
        return this;
    }

    public PackageBuildArgBuilder WithPackageInstalationDir(string packageInstalationDir)
    {
        //
        WithOption(InatallationTargetRootDirOption.NAME, packageInstalationDir);
        return this;
    }

    public PackageBuildArgBuilder WithPackageDestinationDir(string packageInstalationDir)
    {
        WithOption(PackageDestinationDirOption.NAME, packageInstalationDir);
        return this;
    }
}
