using CSharpFunctionalExtensions;
using Gcd.Services;
using MediatR;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;

namespace Gcd.Commands.Config.GetCongi;

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

public static class MediatorExtensions
{
    public static async Task<Result<IReadOnlyList<ConfigProperty>>> GetConfigAsync(this IMediator mediator, IReadOnlyList<ConfigProperty> configProperties, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetConfigRequest(configProperties), cancellationToken);
    public static async Task<Result<IReadOnlyList<ConfigProperty>>> GetConfigAsync(this IMediator mediator, ConfigProperty configProperty, CancellationToken cancellationToken = default)
    => await mediator.Send(new GetConfigRequest(new List<ConfigProperty> { configProperty }), cancellationToken);

}