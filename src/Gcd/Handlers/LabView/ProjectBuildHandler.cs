using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Common;
using Gcd.Handlers.Project.BuildSpec;
using Gcd.LabViewProject;
using Gcd.Model.Config;

namespace Gcd.Handlers.LabView;

public record ProjectBuildRequest(
    LabViewProjectPath LabViewProjPath,
    LabViewPath LabViewPath,
    LabViewPort LabViewPort,
    ProjectOutputDir ProjectOutputDir,
    BuildSpecVersion ProjectVersion
    ) : IRequest<Result<string,Error>>;

public class ProjectBuildHandler(IMediator _mediator,ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<ProjectBuildRequest,Result<string,Error>>
{
    public async Task<Result<string,Error>> Handle(ProjectBuildRequest request, CancellationToken cancellationToken)
    {
        var (projectPath,labViewPath, labViewPort,outputDir,version) = request;
        var cmd = LabViewCliCmdPath.None;

        var specs = await _mediator.BuildSpecListAsync(projectPath);

        var specVal = specs.Value;

        var exeOnlySpec = specVal.Where(x => x.Type.ToLower().Equals("exe"));
        // for now, exe mut be executed before packag, should be done beter way here
        var packageOnlySpec = specVal.Where(x => x.Type.Equals("{E661DAE2-7517-431F-AC41-30807A3BDA38}"));
        

        foreach (var spec in exeOnlySpec)
        {
            var specName = BuildSpecName.Of(spec.Name);
            var specTarget = BuildSpecTarget.Of(spec.Target);
            var specType = BuildSpecType.Of(spec.Type);
            
                // $resolvedPath = $outputPath + "\exe\" +$projectName + "\" + $target + "\" + $buildSpec
            var buildSpecOutputDir = BuildSpecOutputDir.Of(@$"{outputDir.Value}\{spec.Target}\{spec.Name}");
            await _mediator.BuildSpecBuildAsync(projectPath,labViewPath,labViewPort, specName.Value, specTarget.Value, version,buildSpecOutputDir.Value,cancellationToken);
        }
        
        foreach (var spec in packageOnlySpec)
        {
            var specName = BuildSpecName.Of(spec.Name);
            var specTarget = BuildSpecTarget.Of(spec.Target);
            var specType = BuildSpecType.Of(spec.Type);
            
            // $resolvedPath = $outputPath + "\exe\" +$projectName + "\" + $target + "\" + $buildSpec
            var buildSpecOutputDir = BuildSpecOutputDir.Of(@$"{outputDir.Value}\{spec.Target}\{spec.Name}");
            await _mediator.BuildSpecBuildAsync(projectPath,labViewPath,labViewPort, specName.Value, specTarget.Value, version,buildSpecOutputDir.Value,cancellationToken);
        }
        
        return Result.Success<string, Error>("suceess");
    }
}


public static class MediatorExtensions3
{
    public static async Task<Result<string,Error>> ProjectBuildAsync(
        this IMediator mediator,
        LabViewProjectPath projectPath,
        LabViewPath labViewPath,
        LabViewPort labViewPort,
        ProjectOutputDir projectOutputDir,
        BuildSpecVersion projectVersion,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new ProjectBuildRequest(projectPath,labViewPath,labViewPort,projectOutputDir,projectVersion), cancellationToken);
}