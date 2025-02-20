using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Handlers.LabView;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;

namespace Gcd.Commands.LabView;
public sealed class LabViewProjectPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--project-path";
    public Result<LabViewProjectPath,Error> Map() =>
        LabViewProjectPath.Of(this.Value());
}

public sealed class LabViewBuildSpecNameOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--build-spec-name";
    public Result<BuildSpecName,Error> Map() =>
        BuildSpecName.Of(this.Value());
}

public sealed class LabViewBuildSpecTypeOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--build-spec-type";
    public Maybe<string> Map() =>
        Maybe.From(this.Value());
}
public sealed class LabViewBuildSpecTargetOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--build-spec-target";
    public Result<BuildSpecTarget,Error> Map() =>
        BuildSpecTarget.Of(this.Value());
}

public sealed class LabViewBuildSpecVersionOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--build-spec-version";
    public Result<BuildSpecVersion,Error> Map() =>
        BuildSpecVersion.Of(this.Value());
}

public sealed class LabViewProjectVersionOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--project-version";
    public Result<BuildSpecVersion,Error> Map() =>
        BuildSpecVersion.Of(this.Value());
}

public sealed class LabViewBuildSpecOutputDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--build-spec-output-dir";
    public Result<BuildSpecOutputDir,Error> Map() =>
        BuildSpecOutputDir.Of(this.Value());
}

public sealed class ProjectOutputDirOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--project-output-dir";
    public Result<ProjectOutputDir,Error> Map() =>
        ProjectOutputDir.Of(this.Value());
}

public sealed class LabViewViPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--vi-path";
    public Result<LabViewViPath,Error> Map() =>
        LabViewViPath.Of(this.Value());
}

public sealed class LabViewPortOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--labview-port";
    public Result<LabViewPort,Error> Map() =>
        LabViewPort.Of(this.Value());
}

public sealed class LabViewPathOption() : CommandOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--labview-path";
    public Result<LabViewPath,Error> Map() =>
        LabViewPath.Of(this.Value());
}