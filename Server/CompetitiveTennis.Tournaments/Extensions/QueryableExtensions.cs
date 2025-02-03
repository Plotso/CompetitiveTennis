namespace CompetitiveTennis.Tournaments.Extensions;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Models;
using Contracts;

public static class QueryableExtensions
{
    public static IQueryable<T> PageFilterResult<T>(this IQueryable<T> collection, PageQuery query)
        => collection != null ? 
            collection
                .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage) :
            null;
    
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