using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.Nipkg.Replication;

public class ListInstalledParser
{
    public Result<IReadOnlyList<PackageDto>,Error> Parse(string commandOutput)
    {
        var rows = commandOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        List<PackageDto> packageList = new List<PackageDto>();
        foreach (var row in rows)
        {
            var columns = row.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (columns.Length >= 2)
            {
                var packageDto = new PackageDto(columns[0], columns[1]);
                    
                packageList.Add(packageDto);
            }
        }
        return Result.Success<IReadOnlyList<PackageDto>,Error>(packageList.AsReadOnly());
    }
}
