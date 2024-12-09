using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Commands.NipkgPushAzBlobFeedMeta;

public static class MediatorExtensions
{
    public static async Task<Result> PushAzBlobFeedMetaDataAsync(this IMediator mediator, AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPushAzBlobFeedMetaRequest(AzFeedDef, LocalFeedDef), cancellationToken);  
}

