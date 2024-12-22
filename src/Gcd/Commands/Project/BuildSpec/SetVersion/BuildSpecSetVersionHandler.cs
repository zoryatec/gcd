using Gcd.LabViewProject;
using MediatR;

namespace Gcd.Commands.Project.BuildSpec.SetVersion;

public record BuildSpecSetVersionRequest(string projectPath, string buildSpecName, string buildSpecType, string buildSpecTarget, string versionToSet) : IRequest<BuildSpecSetVersioResponse>;
public record BuildSpecSetVersioResponse(string result);

public class BuildSpecSetVersionHandler(ILabViewProjectProvider _labViewProjectProvider)
    : IRequestHandler<BuildSpecSetVersionRequest, BuildSpecSetVersioResponse>
{
    public async Task<BuildSpecSetVersioResponse> Handle(BuildSpecSetVersionRequest request, CancellationToken cancellationToken)
    {
        var maybeProject = _labViewProjectProvider.GetProject(request.projectPath);
        var project = maybeProject.Value;


        var buildSpecMaybe = project.GetBuildSpec(request.buildSpecName, request.buildSpecType, request.buildSpecTarget);
        var buildSpec = buildSpecMaybe.Value;

        var version = BuildSpecVersion.Create(request.versionToSet);

        buildSpec.SetVersion(version.Value);
        _labViewProjectProvider.Save(project);
        return new BuildSpecSetVersioResponse("result");
    }
}