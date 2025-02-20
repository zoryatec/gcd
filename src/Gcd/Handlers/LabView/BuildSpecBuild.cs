using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Project.BuildSpec;
using Gcd.LabViewProject;
using Gcd.Model.Config;

namespace Gcd.Handlers.LabView;

public record BuildSpecBuildRequest(
    LabViewProjectPath LabViewProjPath,
    LabViewPath LabViewPath,
    LabViewPort LabViewPort,
    BuildSpecName BuildSpecName,
    BuildSpecTarget BuildSpecTarget,
    BuildSpecVersion BuildSpecVersion,
    BuildSpecOutputDir BuildSpecOutputDir
    ) : IRequest<Result>;

public class GetConfigHandler(IMediator _mediator,ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecBuildRequest,Result>
{
    public async Task<Result> Handle(BuildSpecBuildRequest request, CancellationToken cancellationToken)
    {
        var (projectPath,labViewPath, labViewPort,specName,specTarget,version,outputDir) = request;
        var cmd = LabViewCliCmdPath.None;

        // set output path
        var maybeProject = _labViewProjectProvider.GetProject(projectPath.Value);
        var project = maybeProject.Value;
        var buildSpecMaybe = project.GetBuildSpec(specName.Value, "EXE", specTarget.Value);
        var buildSpec = buildSpecMaybe.Value;

        // var version = BuildSpecVersion.Create(request.versionToSet);

        buildSpec.SetOutpuDir(outputDir.Value);
        _labViewProjectProvider.Save(project);

        await _mediator.BuildSpecSetVersionAsync(projectPath, specName, "", specTarget, version, cancellationToken);
        return await   _mediator.ExecuteBuildSpecAsync(projectPath, cmd, labViewPath, labViewPort, specName,
                specTarget, cancellationToken);
    }
}


public static class MediatorExtensions2
{
    public static async Task<Result> BuildSpecBuildAsync(
        this IMediator mediator,
        LabViewProjectPath projectPath,
        LabViewPath labViewPath,
        LabViewPort labViewPort,
        BuildSpecName buildSpecName,
        BuildSpecTarget buildSpecTarget,
        BuildSpecVersion buildSpecVersion,
        BuildSpecOutputDir buildSpecOutputDir,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new BuildSpecBuildRequest(projectPath,labViewPath,labViewPort,buildSpecName,buildSpecTarget,buildSpecVersion,buildSpecOutputDir), cancellationToken);
}