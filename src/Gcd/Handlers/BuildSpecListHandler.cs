using System.Text.Json;
using Gcd.CommandHandlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record BuildSpecListRequest(string projectPaht) : IRequest<BuildSpecListResponse>;
public record BuildSpecListResponse(string ProjectPaht);

public class BuildSpecListHandler(IProjectService _projectService)
    : IRequestHandler<BuildSpecListRequest, BuildSpecListResponse>
{
    public async Task<BuildSpecListResponse> Handle(BuildSpecListRequest request, CancellationToken cancellationToken)
    {
        var projectListDto = _projectService.GetBuildSpecList(request.projectPaht);
        string jsonString = JsonSerializer.Serialize(projectListDto);
        return new BuildSpecListResponse(jsonString);
    }
}