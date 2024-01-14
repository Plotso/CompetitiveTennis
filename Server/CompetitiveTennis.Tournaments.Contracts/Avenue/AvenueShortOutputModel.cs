namespace CompetitiveTennis.Tournaments.Contracts.Avenue;

public record AvenueShortOutputModel(int Id, string Name, string Location, string City, string Country, string Details, bool IsVerified, bool IsActive);