﻿using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;

public record PullFeedMetaRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef)
    : IRequest<Result> where TFeedDefinition : IFeedDefinition;