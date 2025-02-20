using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Build;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg;

public class ArgBuilderBuild: ArgBuilder
{
    public ArgBuilderBuild(Arguments arguments) : base(arguments)
    {
        _arguments.Add(UseCmdBuildExtension.NAME);
    }

    public ArgBuilderBuild WithPackageContentDirectory(string packageContentDirectory)
    {
        WithOption(BuilderContentSourceDirOption.NAME, packageContentDirectory);
        return this;
    }
    public ArgBuilderBuild WithPackageName(string packageName)
    {
        WithOption(PackageNameOption.NAME, packageName);
        return this;
    }

    public ArgBuilderBuild WithPackageVersion(string packageVersion)
    {
        WithOption(PackageVersionOption.NAME, packageVersion);
        return this;
    }

    public ArgBuilderBuild WithPackageInstalationDir(string packageInstalationDir)
    {
        //
        WithOption(InatallationTargetRootDirOption.NAME, packageInstalationDir);
        return this;
    }

    public ArgBuilderBuild WithPackageDestinationDir(string packageInstalationDir)
    {
        WithOption(PackageDestinationDirOption.NAME, packageInstalationDir);
        return this;
    }
}
