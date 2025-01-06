using CSharpFunctionalExtensions;
using Gcd.Model.Nipkg.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;

public record PushFeedMetaRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDefinition)
        : IRequest<Result> where TFeedDefinition : IFeedDefinition;