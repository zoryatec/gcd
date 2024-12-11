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

        var controlFileContentResult = await PackageBuilderDefinition.Of(request.PackagePath)
            .Bind(def => ReadTextFile(def.ControlFile));


        string controlFileContent = controlFileContentResult.Value;

        string newVersion = request.PackageVersion.Value;

        // Regular expression to match the line starting with "Version:"
        string pattern = @"^Version:.*$";
        string replacement = $"Version: {newVersion}";

        // Replace the line
        controlFileContent = Regex.Replace(controlFileContent, pattern, replacement, RegexOptions.Multiline);

        var writeResult = await PackageBuilderDefinition.Of(request.PackagePath)
            .Bind(def => WriteTextFile(def.ControlFile, controlFileContent));

        return Result.Success();
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


