using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Nipkg.Replication;
using MediatR;

namespace Gcd.Handlers.Nipkg.Replication;

public record GetSystemPackagesRequest : IRequest<Result>;

public class GetSystemPackagesHandler( IMediator _mediator)
    : IRequestHandler<GetSystemPackagesRequest, Result>
{
    public async Task<Result> Handle(GetSystemPackagesRequest request, CancellationToken cancellationToken)
    {
        var parser = new ListInstalledParser();
        var result  = await _mediator.NipkgGetListInstalledAsync(NipkgCmdPath.None)
            .Bind(output => parser.Parse(output));

        foreach (var package in result.Value)
        {
            var deps = await _mediator.NipkgGetDependenciesAsync(NipkgCmdPath.None, package.Name)
                .Bind(output => parser.Parse(output));
        }
        
        return await Task.FromResult(new Result()); 
    }
}