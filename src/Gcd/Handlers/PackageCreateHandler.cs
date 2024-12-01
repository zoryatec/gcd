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

public record PackageCreateRequest(string PackagePath, string PackageName, string PackageVersion, string PackageDestinationDir) : IRequest<PackageCreateResponse>;
public record PackageCreateResponse(string result);

public class PackageCreateHandler(IMediator _mediator)
    : IRequestHandler<PackageCreateRequest, PackageCreateResponse>
{
    public async Task<PackageCreateResponse> Handle(PackageCreateRequest request, CancellationToken cancellationToken)
    {

        string temporaryDirectory = "C:\\Projects\\AAAATESTPKG";
        var subRequest = new TemplateCreateRequest(temporaryDirectory, request.PackageName, request.PackageVersion, request.PackageDestinationDir);
        var subResponse = await _mediator.Send(subRequest);


        return new PackageCreateResponse("result");
    }
}

