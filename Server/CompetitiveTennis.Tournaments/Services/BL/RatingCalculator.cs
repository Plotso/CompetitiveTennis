namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Configurations;
using Extensions;
using Interfaces.BL;
using Microsoft.Extensions.Options;
using Models.MatchOutcomeHandler.RatingCalculations;
using Tournaments.Data;

public class RatingCalculator : IRatingCalculator
{
    private readonly IOptionsMonitor<RatingCalculatorConfiguration> _configuration;
    private const ushort ScalingFactor = 400; // Used for expected score calculation, adjusting it scales expected score sensitivity
    private const byte GameDifferenceNormalizer = 10; // Normalizes game difference
    private const byte PointDifferenceNormalizer = 50; // Normalizes point difference
    private const int KFactor = 20; // Sensitivity for rating change

    public RatingCalculator(IOptionsMonitor<RatingCalculatorConfiguration> configuration)
    {
        _configuration = configuration;
    }

    public NewRatingPerAccount[] CalculateRatings(MatchResultSummaryWithRatings matchResult, bool isDoubles = false)
    {
        if(matchResult == null || matchResult.Participants.IsNullOrEmpty())
            throw new ArgumentException("Match participants must not be null or empty in order to Calculate new ratings");
        if(matchResult.Outcome == MatchOutcome.NoOutcome) // cannot calculate ratings when the winner of the match is unknown or it finished as a Tie
            return Array.Empty<NewRatingPerAccount>(); 
        return isDoubles ? HandleDoublesRating(matchResult) : HandleSinglesRating(matchResult);
    }

    private NewRatingPerAccount[]  HandleSinglesRating(MatchResultSummaryWithRatings matchResult)
    {
        var homeParticipant = matchResult.Participants.First(p => p.Specifier == DataConstants.ParticipantSpecifiers.Home);
        var awayParticipant = matchResult.Participants.First(p => p.Specifier == DataConstants.ParticipantSpecifiers.Away);

        var homeRating = homeParticipant.IsGuest ? ServiceConstants.DefaultPlayerRating : homeParticipant.Players.Single().PlayerRating;
        var awayRating = awayParticipant.IsGuest ? ServiceConstants.DefaultPlayerRating : awayParticipant.Players.Single().PlayerRating;

        var expectedHomeScore = CalculateExpectedScore(homeRating, awayRating);
        var expectedAwayScore = 1 - expectedHomeScore;

        var actualHomeScore = matchResult.Outcome == MatchOutcome.ParticipantOne ? 1d : 0d;
        var actualAwayScore = 1 - actualHomeScore;

        // Calculate Margin of Victory multiplier
        var movMultiplier = CalculateMarginOfVictory(matchResult.HomeSideResult, matchResult.AwaySideResult);

        // Apply Elo formula for both players
        var homeNewRating = UpdateRating(homeRating, expectedHomeScore, actualHomeScore, movMultiplier);
        var awayNewRating = UpdateRating(awayRating, expectedAwayScore, actualAwayScore, movMultiplier);

        if (homeParticipant.IsGuest && awayParticipant.IsGuest)
            return Array.Empty<NewRatingPerAccount>();
        if (homeParticipant.IsGuest)
            return new NewRatingPerAccount[] {new(awayParticipant.Players.Single().Id, awayNewRating)};
        if (awayParticipant.IsGuest)
            return new NewRatingPerAccount[] {new(homeParticipant.Players.Single().Id, homeNewRating)};
        return new NewRatingPerAccount[] {new(homeParticipant.Players.Single().Id, homeNewRating), new(awayParticipant.Players.Single().Id, awayNewRating)};
    }

    // Handles doubles match rating adjustments
    private NewRatingPerAccount[] HandleDoublesRating(MatchResultSummaryWithRatings matchResult)
    {
        if(!_configuration.CurrentValue.DoublesRatingCalculationEnabled)
            return Array.Empty<NewRatingPerAccount>();
        
        var homeParticipant = matchResult.Participants.First(p => p.Specifier == DataConstants.ParticipantSpecifiers.Home);
        var awayParticipant = matchResult.Participants.First(p => p.Specifier == DataConstants.ParticipantSpecifiers.Away);

        var homeRating = CalculateDoublesCombinedRating(homeParticipant);
        var awayRating = CalculateDoublesCombinedRating(awayParticipant);

        var expectedHomeScore = CalculateExpectedScore(homeRating, awayRating);
        var expectedAwayScore = 1 - expectedHomeScore;

        var actualHomeScore = matchResult.Outcome == MatchOutcome.ParticipantOne ? 1d : 0d;
        var actualAwayScore = 1 - actualHomeScore;

        // Calculate Margin of Victory multiplier
        var movMultiplier = CalculateMarginOfVictory(
            matchResult.HomeSideResult,
            matchResult.AwaySideResult
        );

        // Apply Elo formula to the team rating
        var homeTeamNewRating = UpdateRating(homeRating, expectedHomeScore, actualHomeScore, movMultiplier);
        var awayTeamNewRating = UpdateRating(awayRating, expectedAwayScore, actualAwayScore, movMultiplier);

        // Distribute rating changes among players
        return DistributeDoublesRatingChanges(playersInTeam: 2, homeRating, awayRating, homeTeamNewRating, awayTeamNewRating, homeParticipant, awayParticipant);
    }

    private int CalculateDoublesCombinedRating(ParticipantRatingOutputModel participantRatingOutputModel)
    {
        if(participantRatingOutputModel is null)
            throw new ArgumentNullException($"{nameof(ParticipantRatingOutputModel)} cannot be null");
        if (participantRatingOutputModel.IsGuest)
            return participantRatingOutputModel.Players.Any()
                ? ServiceConstants.DefaultPlayerRating + participantRatingOutputModel.Players.Sum(p => p.DoublesPlayerRating)
                : 2 * ServiceConstants.DefaultPlayerRating; // 2 guests case
        
        return participantRatingOutputModel.Players.Sum(p => p.DoublesPlayerRating);
    }

    // Calculates expected score
    private double CalculateExpectedScore(int playerRating, int opponentRating) 
        => 1 / (1 + Math.Pow(10, (opponentRating - playerRating) / (double)ScalingFactor));

    // Updates rating based on the Elo formula
    private int UpdateRating(int currentRating, double expectedScore, double actualScore, double movMultiplier) 
        => (int)(currentRating + KFactor * movMultiplier * (actualScore - expectedScore));

    // Calculates Margin of Victory multiplier
    protected double CalculateMarginOfVictory(TennisResultInfo homeResult, TennisResultInfo awayResult)
    {
        var setMultiplier = CalculateSetMultiplier(homeResult.SetsWon, awayResult.SetsWon);
        var gameDifference = Math.Abs(homeResult.GamesWon - awayResult.GamesWon) / (double)GameDifferenceNormalizer;
        var pointDifference = Math.Abs(homeResult.PointsWon - awayResult.PointsWon) / (double)PointDifferenceNormalizer;

        return (1 + setMultiplier) * (1 + gameDifference) * (1 + pointDifference);
    }

    // Calculates set multiplier based on sets won
    protected double CalculateSetMultiplier(byte homeSetsWon, byte awaySetsWon)
    {
        var isBestOfFive = IsBestOfFive(homeSetsWon, awaySetsWon);
        var setDifference = Math.Abs(homeSetsWon - awaySetsWon);
        if (setDifference == 0)
            return 0; // No set difference (unlikely in a valid match result)

        if (isBestOfFive)
            return setDifference switch
            {
                1 => 1.0, // Narrow win (3-2)
                2 => 1.2, // Moderate win (3-1)
                _ => 1.5 // Dominant win (3-0)
            };
        
        return setDifference switch
        {
            1 => 1.2,   // Narrow win (ex: 2-1)
            _ => 1.0    // Dominant win (ex: 2-0)
        };
    }

    private bool IsBestOfFive(byte homeSetsWon, byte awaySetsWon)
    {
        var sumOfSets = homeSetsWon + awaySetsWon;
        if (sumOfSets > 3)
            return true;
        var diffOfSets = Math.Abs(homeSetsWon - awaySetsWon); //If the both sum and diff are 3, then one play won by 3-0
        return sumOfSets > 3 || sumOfSets == 3 && diffOfSets == 3;
    }

    protected NewRatingPerAccount[] DistributeDoublesRatingChanges(
        byte playersInTeam,
        int homeTeamOriginalRating,
        int awayTeamOriginalRating,
        int homeTeamNewRating,
        int awayTeamNewRating,
        ParticipantRatingOutputModel homeParticipant,
        ParticipantRatingOutputModel awayParticipant)
    {
        // Calculate average rating adjustment
        var homeRatingAdjustment = (homeTeamNewRating - homeTeamOriginalRating) / playersInTeam;
        var awayRatingAdjustment = (awayTeamNewRating - awayTeamOriginalRating) / playersInTeam;

        var homeRatingAdjustments = AddRatingAdjustments(homeParticipant.Players.ToArray(), homeRatingAdjustment);
        var awayRatingAdjustments = AddRatingAdjustments(awayParticipant.Players.ToArray(), awayRatingAdjustment);

        return homeRatingAdjustments.Concat(awayRatingAdjustments).ToArray();
    }

    private List<NewRatingPerAccount> AddRatingAdjustments(AccountRatingOutputModel[] players, int adjustment)
    {
        var result = new List<NewRatingPerAccount>(players.Length);
        foreach (var player in players)
        {
            var newRating = player.PlayerRating + adjustment;
            result.Add(new NewRatingPerAccount(player.Id, newRating));
        }

        return result;
    }
}