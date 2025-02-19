using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;

namespace Gcd.Handlers.Config;

public record SetConfigRequest(IReadOnlyList<ConfigProperty> ConfigProperties) : IRequest<Result>;

public class SetConfigHandler(IConfigService _config)
    : IRequestHandler<SetConfigRequest, Result>
{
    public async Task<Result> Handle(SetConfigRequest request, CancellationToken cancellationToken)
    {
        var configProperties = request.ConfigProperties;

        //return  await _config.GetAppConfigAsync()
        //     .Map(config => config.WithProperties(configProperties))
        //     .Map(config => _config.SetAppconfig(config));

        return await _config.SetProperties(configProperties);
    }
}