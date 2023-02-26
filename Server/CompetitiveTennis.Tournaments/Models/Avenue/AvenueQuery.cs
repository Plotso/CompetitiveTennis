namespace CompetitiveTennis.Tournaments.Models.Avenue;

using CompetitiveTennis.Models;
using Data.Models.Enums;
using Enums;

public record AvenueQuery(
    string Keyword = "", string City = "", string Country = "",
    SortOptions SortOptions = SortOptions.CreatedAscending, Surface? Surface = null, CourtType? CourtType = null,
    int Page = 1, int ItemsPerPage = 25) : PageQuery(Page, ItemsPerPage);