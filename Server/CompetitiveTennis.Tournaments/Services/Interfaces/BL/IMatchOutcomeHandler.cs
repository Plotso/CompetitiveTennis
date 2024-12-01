namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Contracts.Match;
using Contracts.MatchPeriod;

public interface IMatchOutcomeHandler
{
    /// <summary>
    /// Mark match as ended and calculate ratings of players
    /// </summary>
    Task HandleMatchOutcome(int matchId, MatchResultsInputModel matchResultsInputModel);
}