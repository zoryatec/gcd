using System.ComponentModel;
using System.Text.Json;
using System.Xml;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record BuildSpecSetVersiotRequest(string projectPath, string buildSpecName, string buildSpecType, string buildSpecTarget) : IRequest<BuildSpecListResponse>;
public record BuildSpecSetVersioResponse(string ProjectPaht);

public class BuildSpecSetVersionHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecListRequest, BuildSpecListResponse>
{
    public async Task<BuildSpecListResponse> Handle(BuildSpecListRequest request, CancellationToken cancellationToken)
    {
        var maybeProject = _labViewProjectProvider.GetProject(request.projectPaht);
        var project = maybeProject.Value;

        List<BuildSpecDto> specs = new List<BuildSpecDto>();
        foreach (var buildSpec in project.BuildSpecifications)
        {
            specs.Add(new BuildSpecDto(buildSpec.Name, buildSpec.Type, buildSpec.Target, buildSpec.Version));
        }
        string jsonString = JsonSerializer.Serialize(specs);
        return new BuildSpecListResponse(jsonString);
    }
}