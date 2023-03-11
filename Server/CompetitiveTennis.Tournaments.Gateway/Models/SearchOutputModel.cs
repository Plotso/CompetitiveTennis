namespace CompetitiveTennis.Tournaments.Gateway.Models;

public record SearchOutputModel<TRecord>(IEnumerable<TRecord> Results, int Page, int Total);