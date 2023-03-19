namespace CompetitiveTennis.Tournaments.Gateway.Models.Avenue;

public record SearchAvenueOutputModel(IEnumerable<AvenueOutputModel> Avenues, int Page, int Total);