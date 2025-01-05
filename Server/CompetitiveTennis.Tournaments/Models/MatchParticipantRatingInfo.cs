namespace CompetitiveTennis.Tournaments.Models;

public record MatchParticipantRatingInfo(int MatchId, int ParticipantId, int? PrematchRating, string Specifier);