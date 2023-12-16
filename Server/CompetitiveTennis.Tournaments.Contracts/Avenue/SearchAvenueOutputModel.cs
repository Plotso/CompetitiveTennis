namespace CompetitiveTennis.Tournaments.Contracts.Avenue;

public record SearchAvenueOutputModel(IEnumerable<AvenueOutputModel> Avenues, int Page, int Total);