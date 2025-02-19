using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.Handlers.LabView;
using Gcd.LabViewProject;
using MediatR;

namespace Gcd.Handlers.Project.BuildSpec;

public record BuildSpecDto(string Name, string Type, string Target, string Version);
public record BuildSpecListRequest(LabViewProjectPath projectPaht) : IRequest<Result<IReadOnlyList<BuildSpecDto>,Error>>;


public class BuildSpecListHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecListRequest, Result<IReadOnlyList<BuildSpecDto>,Error>>
{
    public async Task<Result<IReadOnlyList<BuildSpecDto>,Error>> Handle(BuildSpecListRequest request, CancellationToken cancellationToken)
    {
        var maybeProject = _labViewProjectProvider.GetProject(request.projectPaht.Value);
        var project = maybeProject.Value;

        List<BuildSpecDto> specs = new List<BuildSpecDto>();
        foreach (var buildSpec in project.BuildSpecifications)
        {
            specs.Add(new BuildSpecDto(buildSpec.Name, buildSpec.Type, buildSpec.Target, buildSpec.Version));
        }
        // string jsonString = JsonSerializer.Serialize(specs);
        return Result.Success<IReadOnlyList<BuildSpecDto>,Error>(specs);
    }
}

public static class MediatorExtensions3
{
    public static async Task<Result<IReadOnlyList<BuildSpecDto>,Error>> BuildSpecListAsync(
        this IMediator mediator,
        LabViewProjectPath projectPath,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new BuildSpecListRequest(projectPath), cancellationToken);
    
}