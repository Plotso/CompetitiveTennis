namespace CompetitiveTennis.Tournaments.Extensions;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Models;
using Contracts;
using Contracts.Account;
using Data.Models;

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

    public static IQueryable<Account> SortAccounts(this IQueryable<Account> query, AccountSortOptions accountSortOptions)
        => accountSortOptions switch
        {
            
            AccountSortOptions.SinglesRatingAscending => query.OrderBy( a=> a.PlayerRating),
            AccountSortOptions.DoublesRatingDescending => query.OrderByDescending(a => a.DoublesRating),
            AccountSortOptions.DoublesRatingAscending => query.OrderBy(a => a.DoublesRating),
            _ => query.OrderByDescending(a => a.PlayerRating) // SinglesRating descending, default ordering
        };
}