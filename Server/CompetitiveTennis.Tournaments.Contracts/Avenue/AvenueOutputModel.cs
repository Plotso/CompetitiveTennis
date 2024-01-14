namespace CompetitiveTennis.Tournaments.Contracts.Avenue;

using Tournament;

public record AvenueOutputModel(
    int Id,
    string Name,
    string Location,
    string City,
    string Country,
    string Details,
    bool IsVerified,
    bool IsActive,
    List<CourtsInfo> Courts,
    List<TournamentShortInfoOutput> Tournaments);