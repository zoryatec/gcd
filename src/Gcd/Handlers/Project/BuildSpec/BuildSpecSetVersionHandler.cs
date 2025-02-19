using CSharpFunctionalExtensions;
using Gcd.Handlers.LabView;
using Gcd.LabViewProject;
using MediatR;

namespace Gcd.Handlers.Project.BuildSpec;

public record BuildSpecSetVersionRequest(LabViewProjectPath projectPath, BuildSpecName buildSpecName, string buildSpecType, BuildSpecTarget buildSpecTarget, BuildSpecVersion versionToSet) : IRequest<Result>;


public class BuildSpecSetVersionHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecSetVersionRequest, Result>
{
    public async Task<Result> Handle(BuildSpecSetVersionRequest request, CancellationToken cancellationToken)
    {
        var (projPath, buildSpecName, buildSpecType, buildSpecTarget, versionToSet) = request;
        var maybeProject = _labViewProjectProvider.GetProject(projPath.Value);
        var project = maybeProject.Value;


        var buildSpecMaybe = project.GetBuildSpec(buildSpecName.Value, buildSpecType, buildSpecTarget.Value);
        var buildSpec = buildSpecMaybe.Value;

        // var version = BuildSpecVersion.Create(request.versionToSet);

        buildSpec.SetVersion(versionToSet);
        _labViewProjectProvider.Save(project);
        return Result.Success();
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> BuildSpecSetVersionAsync(
        this IMediator mediator,
        LabViewProjectPath projectPath,
        BuildSpecName buildSpecName,
        string buildSpecType,
        BuildSpecTarget buildSpecTarget,
        BuildSpecVersion versionToSet,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new BuildSpecSetVersionRequest(projectPath, buildSpecName, buildSpecType, buildSpecTarget, versionToSet), cancellationToken);
    
}