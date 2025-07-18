using System.Diagnostics.Contracts;
using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;

namespace Gcd.Snapshot;

public class SnapshotSerializerJson : ISnapshotSerializer
{
    public Task<Result<string>> SerializeAsync(global::Gcd.NiPackageManager.Abstractions.Snapshot snapshot, string path)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<global::Gcd.NiPackageManager.Abstractions.Snapshot>> DeserializeAsync(string snapshotJson)
    {
        // Deserialize to your record type
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(snapshotJson));
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var obj = await JsonSerializer.DeserializeAsync<SnapshotDto>(stream, options);
        return MapToSnapshot(obj);
    }

    private Result<global::Gcd.NiPackageManager.Abstractions.Snapshot> MapToSnapshot(Maybe<SnapshotDto> dto)
    {
        if (dto.HasNoValue)
        {
            return Result.Failure<global::Gcd.NiPackageManager.Abstractions.Snapshot>("Snapshot data is null");
        }
        
        var feedDefinitions = MapToFeedDefinition(dto.Value.Feeds);
        var packages = MapToPackageDefinition(dto.Value.Packages);
        var snapshot = new global::Gcd.NiPackageManager.Abstractions.Snapshot(packages, feedDefinitions);
        return Result.Success(snapshot);
    }

    private FeedDefinition MapToFeedDefinition(FeedDefinitionDto dto) => new(dto.Name, dto.Uri);

    [Pure]
    private IReadOnlyList<FeedDefinition> MapToFeedDefinition(IReadOnlyList<FeedDefinitionDto> dto) =>
        dto.Select(MapToFeedDefinition).ToList();


    private PackageDefinition MapToPackageDefinition(PackageDefinitionDto dto)
    {
        return new PackageDefinition(dto.Package,
            dto.Version,
            dto.Description,
            dto.Depends,
            dto.StoreProduct,
            dto.UserVisible,
            dto.Section);
    }

    private IReadOnlyList<PackageDefinition> MapToPackageDefinition(IReadOnlyList<PackageDefinitionDto> dto) =>
        dto.Select(MapToPackageDefinition).ToList();
}