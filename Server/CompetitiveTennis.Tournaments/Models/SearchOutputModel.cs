namespace CompetitiveTennis.Tournaments.Models;

public record SearchOutputModel<TRecord>(IEnumerable<TRecord> Results, int Page, int Total);