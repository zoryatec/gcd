using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuilderInit;

public record PackageBuilderInitRequest(PackageContentDir PackagePath, PackageName PackageName, PackageVersion PackageVersion, PackageInstalationDir PackageInstalationDir) : IRequest<Result>;

public class TemplateCreateHandler()
    : IRequestHandler<PackageBuilderInitRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderInitRequest request, CancellationToken cancellationToken)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath, request.PackagePath.Value);

        if (Directory.Exists(packageDirectoryPath))
        {
            // Delete the directory
            Directory.Delete(packageDirectoryPath, true);
        }


        Directory.CreateDirectory(packageDirectoryPath);


        string debianbinaryFilePath = Path.Combine(packageDirectoryPath, "debian-binary");
        File.WriteAllText(debianbinaryFilePath, "2.0");


        string dataDirectoryPath = Path.Combine(packageDirectoryPath, "data");
        Directory.CreateDirectory(dataDirectoryPath);

        string windPath = request.PackageInstalationDir.Value.Replace('/', '\\');
        string destinationDirectory = Path.Combine(dataDirectoryPath, windPath);
        Directory.CreateDirectory(destinationDirectory);

        string controlDirectoryPath = Path.Combine(packageDirectoryPath, "control");
        Directory.CreateDirectory(controlDirectoryPath);


        var controlFileContent =
$@"Architecture: windows_x64
Homepage: zoryatec.com
Maintainer: Zoryatec
Description: package descritpion
XB-Plugin: file
XB-UserVisible: yes
XB-StoreProduct: yes
XB-Section: Application Software
Package: {request.PackageName.Value}
Version: {request.PackageVersion.Value}
Depends: 
";
        string controlFilePath = Path.Combine(controlDirectoryPath, "control");
        File.WriteAllText(controlFilePath, controlFileContent);

        var instructionFileContent =
 @"<instructions>
	<targetAttributes readOnly=""allWritable""/>
    <customExecutes>
        <customExecute root=""BootVolume"" step=""install"" schedule=""post"" exeName=""Program Files\gcd\gcd.exe"" arguments=""system add-to-user-path --path C:\PROGRA~1\gcd"" />
    </customExecutes>
</instructions>
";
        string instructionFilePath = Path.Combine(dataDirectoryPath, "instructions");
        File.WriteAllText(instructionFilePath, instructionFileContent);


        return Result.Success();
    }
}

