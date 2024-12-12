using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuilserSetVersion;

public record PackageBuilderSetVersionRequest(PackageBuilderRootDir PackagePath, PackageVersion PackageVersion) : IRequest<Result>;

public class PackageBuilderSetVersionHandler()
    : IRequestHandler<PackageBuilderSetVersionRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderSetVersionRequest request, CancellationToken cancellationToken)
    {
        var (rootDir, packageVersion) = request;
        var pckDefinition = PackageBuilderDefinition.Of(rootDir);

        return  await pckDefinition
            .Bind(def => ReadTextFile(def.ControlFile))
            .Bind(fileContent => ControlFileContent.Of(fileContent))
            .Map(controlFile => controlFile with { Version = packageVersion })
            .Bind(controlFile => WriteTextFile(pckDefinition.Value.ControlFile, controlFile.Content));
    }


    private async Task<Result<string>> ReadTextFile(LocalFilePath filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            string content = File.ReadAllText(filePath.Value);
            return Result.Success(content);
        }
        catch (Exception e) 
        {
            return Result.Failure<string>($"{e.Message}");
        }
    }

    private async Task<Result<string>> WriteTextFile(LocalFilePath filePath, string content, CancellationToken cancellationToken = default)
    {
        try
        {
            File.WriteAllText(filePath.Value, content);
            return Result.Success(content);
        }
        catch (Exception e)
        {
            return Result.Failure<string>($"{e.Message}");
        }
    }

}


