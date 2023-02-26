namespace CompetitiveTennis.Tournaments.Extensions;

using Data.Models.Enums;
using Models;
using Models.Enums;

public static class CourtsInfoExtensions
{
    public static List<CourtsInfo> UnifyCourtsInfo(this List<CourtsInfo> courtsInfos)
    {
        var itemsBySurface = courtsInfos
            .GroupBy(c => c.Surface)
            .ToDictionary(g => g.Key, g => g.Count());

        var result = new List<CourtsInfo>();
        if (itemsBySurface.Any(i => i.Value > 1))
        {
            var validKeys = itemsBySurface.Where(i => i.Value == 1).Select(g => g.Key).ToList();
            var validCourtInfos = courtsInfos.Where(c => validKeys.Contains(c.Surface)).ToList();
            
            var duplicatedSurfaces = itemsBySurface.Where(i => i.Value > 1).Select(g => g.Key).ToList();
            foreach (var surface in duplicatedSurfaces)
            {
                var courtsWithSurface = courtsInfos.Where(c => c.Surface == surface).ToList();
                var firstEntry = courtsWithSurface.First();
                firstEntry.AvailableCourtsByType = MergeAvailableCourtsByType(courtsWithSurface);
                validCourtInfos.Add(firstEntry);
            }
            result.AddRange(validCourtInfos);
        }
        else
        {
            result = courtsInfos;
        }

        var itemsBySurfaceAndCourtTYpe = result
            .SelectMany(courtsInfo => courtsInfo.AvailableCourtsByType
                .Select(kvp => new { courtsInfo.Surface, CourtType = kvp.Key, Count = kvp.Value }))
            .GroupBy(item => new { item.Surface, item.CourtType })
            .ToDictionary(
                group => new { group.Key.Surface, group.Key.CourtType },
                group => group.Count());
        if (itemsBySurfaceAndCourtTYpe.Any(i => i.Value > 1))
        {
            var surfacesToFix = itemsBySurfaceAndCourtTYpe.Where(i => i.Value > 1);
            var processedSurfaces = new HashSet<Surface>();
            foreach (var surfaceToFix in surfacesToFix)
            {
                if (processedSurfaces.Contains(surfaceToFix.Key.Surface)) 
                    continue;;
                var resultEntry = result.FirstOrDefault(r => r.Surface == surfaceToFix.Key.Surface);
                if (resultEntry != null)
                {
                    resultEntry.AvailableCourtsByType = MergeAvailableCourtsByType(new List<CourtsInfo>(1) {resultEntry});
                    processedSurfaces.Add(resultEntry.Surface);
                }
            }
        }

        return result;
    }
    
    /// <summary>
    /// Merge AvailableCourtsByType of multiple <see cref="CourtsInfo"/> entries into a single Dictionary. The values for each courtType is considered the highest value seen inside the all entries for the given type.
    /// </summary>
    public static Dictionary<CourtType, int> MergeAvailableCourtsByType(this List<CourtsInfo> courtsList)
    {
        var result = new Dictionary<CourtType, int>();
        foreach (var courtsInfo in courtsList)
        {
            foreach (var kvp in courtsInfo.AvailableCourtsByType)
            {
                if (result.TryGetValue(kvp.Key, out int existingValue))
                {
                    if (kvp.Value > existingValue)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
        }
        return result;
    }
}