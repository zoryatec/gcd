using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Commands.NipkgPullFeedMeta
{
    public static class MediatorExtensions
    {
        public static async Task<Result> PullAzBlobFeedMetaDataAsync(this IMediator mediator, AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef, CancellationToken cancellationToken = default)
            => await mediator.Send(new NipkgPullFeedMetaRequest(AzFeedDef, LocalFeedDef), cancellationToken);  
    }
}
