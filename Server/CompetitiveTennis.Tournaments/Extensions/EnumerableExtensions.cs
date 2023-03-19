namespace CompetitiveTennis.Tournaments.Extensions;

using Models;

public static class EnumerableExtensions
{
    public static IEnumerable<T> PageFilterResult<T>(this IEnumerable<T> collection, PageQuery query)
        => collection != null ? 
            collection
                .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage) :
            new List<T>();
}