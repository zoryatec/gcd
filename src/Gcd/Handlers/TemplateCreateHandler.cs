using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record TemplateCreateRequest(string PackagePath, string PackageName, string PackageVersion, string PackageDestinationDir) : IRequest<TemplateCreateResponse>;
public record TemplateCreateResponse(string result);

public class TemplateCreateHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<TemplateCreateRequest, TemplateCreateResponse>
{
    public async Task<TemplateCreateResponse> Handle(TemplateCreateRequest request, CancellationToken cancellationToken)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath,request.PackagePath);

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

        string windPath = request.PackageDestinationDir.Replace('/', '\\');
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
Package: {request.PackageName}
Version: {request.PackageVersion}
Depends: 
";

        string controlFilePath = Path.Combine(controlDirectoryPath, "control");
        File.WriteAllText(controlFilePath, controlFileContent);

        return new TemplateCreateResponse("result");
    }
}

