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

        test();

        return  await pckDefinition
            .Bind(def => ReadTextFileAsync(def.ControlFile))
            .Bind(fileContent => ControlFileContent.Of(fileContent))
            .Map(controlFile => controlFile with { Version = packageVersion })
            .Bind(controlFile => WriteTextFile(pckDefinition.Value.ControlFile, controlFile.Content));
    }
    
    private void test()
    {
        List<ControlFileProperty> controlFileProperties = new List<ControlFileProperty>();
        controlFileProperties.Add(PackageArchitecture.Of("dddd").Value);
        controlFileProperties.Add(PackageHomePage.Of("adsfdsfds").Value);
        controlFileProperties.Add(PackageMaintainer.Of("dfdfd").Value);

        var cfc = ControlFileContent.Default;

        foreach(var prop in controlFileProperties) 
        {
            cfc = cfc.WithProperty(prop);
        }


    }


    
    private async Task<Result<string>> ReadTextFileAsync(LocalFilePath filePath, CancellationToken cancellationToken = default) =>
        ReadTextFile(filePath);

    private Result<string> ReadTextFile(LocalFilePath filePath) =>
        Result.Try(() => File.ReadAllText(filePath.Value), ex => ex.Message);

    private async Task<Result> WriteTextFileAsync(LocalFilePath filePath, string content, CancellationToken cancellationToken = default) =>
        WriteTextFile(filePath, content);

    private Result WriteTextFile(LocalFilePath filePath, string content) =>
        Result.Try(() => File.WriteAllText(filePath.Value, content), ex => ex.Message);
}


