namespace CompetitiveTennis.Tournaments.Models.Avenue;

public record SearchAvenueOutputModel(IEnumerable<AvenueOutputModel> Avenues, int Page, int Total);