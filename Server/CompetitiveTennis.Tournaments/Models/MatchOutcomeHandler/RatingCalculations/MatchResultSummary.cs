namespace CompetitiveTennis.Tournaments.Models.MatchOutcomeHandler.RatingCalculations;

using CompetitiveTennis.Data.Models.Enums;

public record MatchResultSummary(
    int Id, 
    MatchOutcome Outcome,
    TennisResultInfo HomeSideResult,
    TennisResultInfo AwaySideResult);


public record MatchResultSummaryWithRatings(
    int Id, 
    MatchOutcome Outcome,
    TennisResultInfo HomeSideResult,
    TennisResultInfo AwaySideResult,
    IEnumerable<ParticipantRatingOutputModel> Participants);
    
public record TennisResultInfo(byte SetsWon, ushort GamesWon, ushort PointsWon);