namespace CompetitiveTennis.Tournaments.UnitTests;

using Configurations;
using Microsoft.Extensions.Options;
using Models.MatchOutcomeHandler.RatingCalculations;
using Services.BL;

public class RatingCalculatorMoq : RatingCalculator
{
    public RatingCalculatorMoq(IOptionsMonitor<RatingCalculatorConfiguration> configuration) : base(configuration)
    {
    }
    
    public double CalculateSetMultiplierMoq(byte homeSetsWon, byte awaySetsWon)
        => CalculateSetMultiplier(homeSetsWon, awaySetsWon);

    public double CalculateMarginOfVictoryMoq(TennisResultInfo homeResult, TennisResultInfo awayResult)
        => CalculateMarginOfVictory(homeResult, awayResult);

    public NewRatingPerAccount[] DistributeDoublesRatingChangesMoq(
        byte playersInTeam,
        int homeTeamOriginalRating,
        int awayTeamOriginalRating,
        int homeTeamNewRating,
        int awayTeamNewRating,
        ParticipantRatingOutputModel homeParticipant,
        ParticipantRatingOutputModel awayParticipant)
        => DistributeDoublesRatingChanges(playersInTeam, homeTeamOriginalRating, awayTeamOriginalRating,
            homeTeamNewRating, awayTeamNewRating, homeParticipant, awayParticipant);
}