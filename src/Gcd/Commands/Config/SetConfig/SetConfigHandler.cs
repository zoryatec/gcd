using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;

namespace Gcd.Commands.Config.SetConfig;

public record SetConfigRequest(IReadOnlyList<ConfigProperty> ConfigProperties) : IRequest<Result>;

public class SetConfigHandler(IConfigService _config)
    : IRequestHandler<SetConfigRequest, Result>
{
    public async Task<Result> Handle(SetConfigRequest request, CancellationToken cancellationToken)
    {
        var configProperties = request.ConfigProperties;

       return  await _config.GetAppConfigAsync()
            .Map(config => config.WithProperties(configProperties))
            .Map(config => _config.SetAppconfig(config));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> SetConfigAsync(this IMediator mediator, IReadOnlyList<ConfigProperty> configProperties, CancellationToken cancellationToken = default)
        => await mediator.Send(new SetConfigRequest(configProperties), cancellationToken);
    public static async Task<Result> SetConfigAsync(this IMediator mediator, ConfigProperty configProperty, CancellationToken cancellationToken = default)
    => await mediator.Send(new SetConfigRequest(new List<ConfigProperty> { configProperty }), cancellationToken);

}