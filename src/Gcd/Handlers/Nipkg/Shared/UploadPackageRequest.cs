using CSharpFunctionalExtensions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;

public record UploadPackageRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, PackageLocalFilePath PackageFilePath)
    : IRequest<Result> where TFeedDefinition : IFeedDefinition;
