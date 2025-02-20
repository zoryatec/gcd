using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;

namespace Gcd.Handlers.Config;

public record GetConfigRequest(IReadOnlyList<ConfigProperty> ConfigProperties) : IRequest<Result<IReadOnlyList<ConfigProperty>>>;

public class GetConfigHandler(IConfigService _config)
    : IRequestHandler<GetConfigRequest, Result<IReadOnlyList<ConfigProperty>>>
{
    public async Task<Result<IReadOnlyList<ConfigProperty>>> Handle(GetConfigRequest request, CancellationToken cancellationToken)
    {
        var configProperties = request.ConfigProperties;

        return await _config.GetProperties(configProperties);
    }
}

