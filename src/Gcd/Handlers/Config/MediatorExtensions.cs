using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using MediatR;


namespace Gcd.Handlers.Config;
public static class MediatorExtensions
{
    public static async Task<Result<IReadOnlyList<ConfigProperty>>> GetConfigAsync(this IMediator mediator, IReadOnlyList<ConfigProperty> configProperties, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetConfigRequest(configProperties), cancellationToken);
    public static async Task<Result<IReadOnlyList<ConfigProperty>>> GetConfigAsync(this IMediator mediator, ConfigProperty configProperty, CancellationToken cancellationToken = default)
    => await mediator.Send(new GetConfigRequest(new List<ConfigProperty> { configProperty }), cancellationToken);

    public static async Task<Result> SetConfigAsync(this IMediator mediator, IReadOnlyList<ConfigProperty> configProperties, CancellationToken cancellationToken = default)
    => await mediator.Send(new SetConfigRequest(configProperties), cancellationToken);
    public static async Task<Result> SetConfigAsync(this IMediator mediator, ConfigProperty configProperty, CancellationToken cancellationToken = default)
    => await mediator.Send(new SetConfigRequest(new List<ConfigProperty> { configProperty }), cancellationToken);

}