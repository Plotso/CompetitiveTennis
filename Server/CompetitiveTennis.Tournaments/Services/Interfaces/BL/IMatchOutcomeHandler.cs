namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using Contracts.MatchPeriod;

public interface IMatchOutcomeHandler
{
    /// <summary>
    /// Update match outcome, recalculate player ratings & update participants for successor match
    /// </summary>
    Task HandleMatchOutcome(int matchId, MatchResultsInputModel matchResultsInputModel);
    /// <summary>
    /// Update match outcome, recalculate player ratings & update participants for successor match
    /// </summary>
    Task HandleMatchOutcome(int matchId, MatchCustomConditionResultInputModel matchCustomConditionResultInputModel);

    Task<bool> IsChangeOfWinnerForMatchWithAlreadyStartedSuccessorMatch(int matchId, MatchResultsInputModel matchResultsInputModel);
    Task<bool> HasSuccessorMatchStarted(int matchId);
    Task<bool> IsChangeOfWinnerForMatchWithAlreadyStartedSuccessorMatch(int matchId, MatchOutcome updatedMatchOutcome);
}