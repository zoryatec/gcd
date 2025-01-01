using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Model.FeedDefinition;
using MediatR;

namespace Gcd.Handlers.Nipkg.Shared;

public record UploadPackageRequest<TFeedDefinition>(TFeedDefinition FeedDefinition, PackageFilePath PackageFilePath)
    : IRequest<Result> where TFeedDefinition : IFeedDefinition;
