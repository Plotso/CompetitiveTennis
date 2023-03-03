namespace CompetitiveTennis.Tournaments.Extensions;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Models;

public static class QueryableExtensions
{
    public static IQueryable<T> SortQuery<T>(this IQueryable<T> query, SortOptions options) 
        where T : BaseModel<int>
        => options switch
        {
            SortOptions.CreatedDescending => query.OrderByDescending( a=> a.CreatedOn),
            SortOptions.UpdatedAscending => query.OrderBy(a => a.ModifiedOn),
            SortOptions.UpdatedDescending => query.OrderByDescending(a => a.ModifiedOn),
            _ => query // CreatedAscending, default ordering
        };
}