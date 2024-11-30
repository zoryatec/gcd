using System.ComponentModel;
using System.Text.Json;
using System.Xml;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record BuildSpecListRequest(string projectPaht) : IRequest<BuildSpecListResponse>;
public record BuildSpecListResponse(string ProjectPaht);

public class BuildSpecListHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecListRequest, BuildSpecListResponse>
{
    public async Task<BuildSpecListResponse> Handle(BuildSpecListRequest request, CancellationToken cancellationToken)
    {
        var maybeProject = _labViewProjectProvider.GetProject(request.projectPaht);
        var project = maybeProject.Value;

        List<BuildSpecDto> specs = new List<BuildSpecDto>();
        foreach (var buildSpec in project.BuildSpecifications)
        {
            specs.Add(new BuildSpecDto(buildSpec.GetName(), buildSpec.GetType(), ""));
        }
        string jsonString = JsonSerializer.Serialize(specs);
        return new BuildSpecListResponse(jsonString);
    }
}