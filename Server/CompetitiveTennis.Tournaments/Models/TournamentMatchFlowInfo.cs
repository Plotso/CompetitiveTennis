namespace CompetitiveTennis.Tournaments.Models;

public record TournamentMatchFlowInfo(int Id, IEnumerable<MatchFlowOutput> MatchFlows);