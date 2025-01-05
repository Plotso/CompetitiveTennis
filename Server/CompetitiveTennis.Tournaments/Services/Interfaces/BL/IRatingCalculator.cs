namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Models.MatchOutcomeHandler.RatingCalculations;

public interface IRatingCalculator
{
    NewRatingPerAccount[] CalculateRatings(MatchResultSummaryWithRatings matchResult, bool isDoubles = false);
}