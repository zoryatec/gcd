using CSharpFunctionalExtensions;
using MediatR;

namespace Gcd.Handlers.Tools;

public record AddToPathWithFallbackRequest(string PathToAdd) : IRequest<Result>;


public class AddToPathWithFallbackHandler(IMediator mediator)
    : IRequestHandler<AddToPathWithFallbackRequest, Result>
{
    public async Task<Result> Handle(AddToPathWithFallbackRequest request, CancellationToken cancellationToken)
    {
        return await mediator.AddToSystemPath(request.PathToAdd, cancellationToken)
            .OnFailureCompensate(_ => mediator.AddToUserPath(request.PathToAdd, cancellationToken));
    }
}

