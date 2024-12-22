using System.Text.Json;
using Gcd.LabViewProject;
using MediatR;

namespace Gcd.Commands.Project.BuildSpec.List;

public record BuildSpecDto(string Name, string Type, string Target, string Version);
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
            specs.Add(new BuildSpecDto(buildSpec.Name, buildSpec.Type, buildSpec.Target, buildSpec.Version));
        }
        string jsonString = JsonSerializer.Serialize(specs);
        return new BuildSpecListResponse(jsonString);
    }
}