namespace CompetitiveTennis.Tournaments.Models;

public record MatchFlowOutput(int Id, DateTime CreatedOn, DateTime? ModifiedOn, bool IsHome, int MatchId, int SuccessorMatchId, int TournamentId);