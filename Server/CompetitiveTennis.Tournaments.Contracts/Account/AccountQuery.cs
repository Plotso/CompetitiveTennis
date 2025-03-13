namespace CompetitiveTennis.Tournaments.Contracts.Account;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Models;
using CompetitiveTennis.Tournaments.Contracts.Enums;

public record AccountQuery(
    string Keyword = "", 
    SortOptions SortOptions = SortOptions.CreatedAscending, AccountSortOptions? AdditionalSortOptions = null,
    int Page = 1, int ItemsPerPage = 25) : PageQuery(Page, ItemsPerPage);
    
public enum AccountSortOptions
{
    SinglesRatingDescending,
    SinglesRatingAscending,
    DoublesRatingDescending,
    DoublesRatingAscending
}