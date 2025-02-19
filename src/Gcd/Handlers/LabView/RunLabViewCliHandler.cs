

using System.Diagnostics;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LabViewProject;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.PackageBuilder;
using MediatR;

namespace Gcd.Handlers.LabView;




public record RunLabViewCliRequest(string[] arguments, LabViewCliCmdPath cmd, NoOfReTry NoOfReTry) : IRequest<Result<string>>;

public class RunLabViewCliHandler()
    : IRequestHandler<RunLabViewCliRequest, Result<string>>
{
    public async Task<Result<string>> Handle(RunLabViewCliRequest request, CancellationToken cancellationToken)
    {
        var (arguments, cmd, noOfReTry) = request;
        if (cmd == LabViewCliCmdPath.None) // cmd path from arguments not exist
        {
            cmd = LabViewCliCmdPath.InPath;
        }
        
        var result = RunLabViewCli(cmd, arguments);
        if (result.IsFailure)
        {
            for(int i=0; i < noOfReTry.Value; i++)
            {
                result = RunLabViewCli(cmd, arguments);
                if(result.IsSuccess) return result;
            }
        }
        return result;
    }
    
    private Result<string> RunLabViewCli( LabViewCliCmdPath cmd,params string[] args)
    {
        var arguments = string.Join(" ", args);
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = cmd.Value,  
            RedirectStandardOutput = true, 
            RedirectStandardError = true,  
            UseShellExecute = false,    
            CreateNoWindow = true
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
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                
                if (!string.IsNullOrEmpty(errors))  return Result.Failure<string>(errors);
                return Result.Success(output);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.Message);
        }
    }
}


public static class MediatorExtensions
{
    public static async Task<Result<string>> RunLabViewCliRequestAsync(this IMediator mediator, string[] arguments, LabViewCliCmdPath cmd, NoOfReTry noOfReTry, CancellationToken cancellationToken = default)
        => await mediator.Send(new RunLabViewCliRequest(arguments, cmd,noOfReTry), cancellationToken);
    
    public static async Task<Result<string>> ExecuteBuildSpecAsync(
        this IMediator mediator,
        LabViewProjectPath projectPath,
        LabViewCliCmdPath cmd,
        LabViewPath lvPath,
        LabViewPort lvPort,
        BuildSpecName buildSpecName,
        BuildSpecTarget buildSpecTarget,
        CancellationToken cancellationToken = default)
    {
        var noOfRetry = NoOfReTry.Of(2);
        return await mediator.RunLabViewCliRequestAsync([
            "-OperationName", "ExecuteBuildSpec",
            "-PortNumber", lvPort.ToString(),
            "-LabVIEWPath", lvPath.Value,
            "-ProjectPath", projectPath.Value,
            "-TargetName", buildSpecTarget.Value,
            "-BuildSpecName", buildSpecName.Value
        ], cmd,noOfRetry.Value, cancellationToken);
    }
    
    public static async Task<Result<string>> RunViAsync(this IMediator mediator, 
        LabViewViPath viPath,
        IReadOnlyList<string> arguments,
        LabViewCliCmdPath cmd,
        LabViewPath lvPath,
        LabViewPort lvPort,
        CancellationToken cancellationToken = default)
    {
        //    labviewcli -Verbosity Diagnostic -LogFilePath $logFilePath -VIPath $viPath $arguments 
        var noOfRetry = NoOfReTry.Of(2);
        var baseArgs =
            new string[]
            {
                "-OperationName", "RunVI",
                "-PortNumber", lvPort.ToString(),
                "-LabVIEWPath", lvPath.Value,
                "-VIPath", viPath.Value
            };
        var args = baseArgs.Concat(arguments).ToArray();
        return await mediator.RunLabViewCliRequestAsync(args, cmd,noOfRetry.Value, cancellationToken);
    }
}

public record LabViewProjectPath : ILocalFilePath
{
    public LabViewProjectPath(LocalDirPath directory, FileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }

    public static Result<LabViewProjectPath,Error> Of(Maybe<string> packagePathOrNothing) =>
        LocalFilePath.Of(packagePathOrNothing)
            .Bind(lfp => LabViewProjectPath.Of(lfp));
        
    private static Result<LabViewProjectPath,Error> Of(LocalFilePath localFilePath) =>
            new LabViewProjectPath(localFilePath.Directory, localFilePath.FileName);

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }
    
    public string ProjectName { get{return FileName.ToString().Replace(".lvproj","");} }

    public string Value  => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;
}

public record LabViewPath : ILocalFilePath
{
    public LabViewPath(LocalDirPath directory, FileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }

    public static Result<LabViewPath,Error> Of(Maybe<string> value) =>
        LocalFilePath.Of(value)
            .Bind(lfp => LabViewPath.Of(lfp));
        
    private static Result<LabViewPath,Error> Of(LocalFilePath localFilePath) =>
        new LabViewPath(localFilePath.Directory, localFilePath.FileName);

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }

    public string Value  => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;
}

public sealed record BuildSpecOutputDir : ILocalDirPath
{
    public static Result<BuildSpecOutputDir,Error> Of(Maybe<string> value)
    {
        var locDir = LocalDirPath.Of(value);
        return Result.Success<BuildSpecOutputDir,Error>(new BuildSpecOutputDir(locDir.Value));
    }

    private BuildSpecOutputDir(LocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
    public string Value => DirPath.Value;

    private ILocalDirPath DirPath { get; }
}

public sealed record ProjectOutputDir : ILocalDirPath
{
    public static Result<ProjectOutputDir,Error> Of(Maybe<string> value)
    {
        var locDir = LocalDirPath.Of(value);
        return Result.Success<ProjectOutputDir,Error>(new ProjectOutputDir(locDir.Value));
    }

    private ProjectOutputDir(LocalDirPath dirPath)
    {
        DirPath = dirPath;
    }
    public string Value => DirPath.Value;

    private ILocalDirPath DirPath { get; }
}

public record LabViewPort 
{
    private LabViewPort(int value) => Value = value;


    public static Result<LabViewPort, Error> Of(Maybe<string> value)
    {
        var res = value.ToResult("Port cannot be empty");
    
        var va = int.Parse(res.Value.ToString());
        return Result.Success<LabViewPort,Error>(new LabViewPort(va));
    }
    public int Value { get; }

    public override string ToString() => Value.ToString();
}

public record BuildSpecName 
{
    private BuildSpecName(string value) => Value = value;
    public static Result<BuildSpecName, Error> Of(Maybe<string> value) => value
            .ToResult("Port cannot be empty")
            .Map(val => new BuildSpecName(val))
            .MapError(err => new Error(err));
    public string Value { get; }
    public override string ToString() => Value;
}

public record BuildSpecType
{
    private BuildSpecType(string value) => Value = value;
    public static Result<BuildSpecType, Error> Of(Maybe<string> value) => value
        .ToResult("BuildSpecType be empty")
        .Map(val => new BuildSpecType(val))
        .MapError(err => new Error(err));
    public string Value { get; }
    public override string ToString() => Value;
}

// public record BuildSpecVersion
// {
//     private BuildSpecVersion(string value) => Value = value;
//     public static Result<BuildSpecVersion, Error> Of(Maybe<string> value) => value
//         .ToResult("BuildSpecType be empty")
//         .Map(val => new BuildSpecVersion(val))
//         .MapError(err => new Error(err));
//     public string Value { get; }
//     public override string ToString() => Value;
// }

public record BuildSpecTarget
{
    private BuildSpecTarget(string value) => Value = value;
    public static Result<BuildSpecTarget, Error> Of(Maybe<string> value) => value
        .ToResult("Port cannot be empty")
        .Map(val => new BuildSpecTarget(val))
        .MapError(err => new Error(err));
    public string Value { get; }
    public override string ToString() => Value;
}

public record NoOfReTry 
{
    private NoOfReTry(int value) => Value = value;

    public static Result<NoOfReTry, Error> Of(int value)
    {
        return Result.Success<NoOfReTry,Error>(new NoOfReTry(value));
    }

    public static Result<NoOfReTry, Error> Of(Maybe<string> value)
    {
        var res = value.ToResult("NoOfReTry cannot be empty");
    
        var va = int.Parse(res.Value.ToString());
        return Result.Success<NoOfReTry,Error>(new NoOfReTry(va));
    }
    public int Value { get; }

    public override string ToString() => Value.ToString();
}


public record LabViewViPath : ILocalFilePath
{
    public LabViewViPath(LocalDirPath directory, FileName fileName) 
    {
        Directory = directory;
        FileName = fileName;
    }

    public static Result<LabViewViPath,Error> Of(Maybe<string> packagePathOrNothing) =>
        LocalFilePath.Of(packagePathOrNothing)
            .Bind(lfp => LabViewViPath.Of(lfp));
        
    private static Result<LabViewViPath,Error> Of(LocalFilePath localFilePath) =>
        new LabViewViPath(localFilePath.Directory, localFilePath.FileName);

    public LocalDirPath Directory { get; }
    public FileName FileName { get; }

    public string Value  => Path.Combine(Directory.Value, FileName.Value);

    public override string ToString() => Value;
}




public record LabViewCliCmdPath
{
    public static LabViewCliCmdPath None = new LabViewCliCmdPath("unset");
    public static LabViewCliCmdPath InPath = new LabViewCliCmdPath("labviewcli");

    public static Result<LabViewCliCmdPath> Of(Maybe<string> maybeValue)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;

        return maybeValue.ToResult("FilePath should not be empty")
            .Ensure(packagePath => packagePath != string.Empty, "FilePath  should not be empty")
            .Map(packagePath => new LabViewCliCmdPath(packagePath));
    }

    private LabViewCliCmdPath(string path)   {Value = path; }

    public string Value { get; }
}