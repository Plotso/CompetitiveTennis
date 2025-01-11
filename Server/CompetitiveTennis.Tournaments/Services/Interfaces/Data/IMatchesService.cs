namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using CompetitiveTennis.Tournaments.Data.Models;
using Models;
using Models.MatchOutcomeHandler.RatingCalculations;

public interface IMatchesService : IDataService<Match>
{
    Task<IEnumerable<MatchOutputModel>> Query(MatchQuery query);
    
    ValueTask<int> Total(MatchQuery query);
    
    Task<IEnumerable<MatchOutputModel>> GetAll();
    
    Task<MatchOutputModel> Get(int id);
    Task<SlimMatchOutputModel> GetForRatingCalculations(int id);

    Task<int?> GetTournamentIdForMatch(int matchId);

    Task<Match> GetInternal(int id);
    
    Task<IEnumerable<MatchParticipantRatingInfo>> GetMatchParticipantsInfo(int matchId);
    /// <summary>
    /// Check whether match status is != NotStarted or whether it has any period scores
    /// </summary>
    Task<bool?> HasMatchStarted(int id);
    Task<bool?> HasMatchEnded(int id);
    Task<MatchOutcome?> GetMatchOutcome(int id);
    Task<bool?> IsDoubles(int id);

    /// <summary>
    /// Create empty match without competitors with the goal for them to be populated on later stage
    /// </summary>
    Task<int> CreateEmptyMatch(MatchInputModel input, Tournament tournament);
    
    Task<int> Create(MatchInputModel input, Tournament tournament, Participant homeParticipant, Participant awayParticipant);

    Task<int> AddMatchFlow(int tournamentId, int matchId, int successorMatchId, bool isWinnerHome);
    Task<MatchFlow?> GetMatchFlow(int matchId);

    Task<bool> UpdateParticipant(int id, Participant newParticipant, bool isHome);

    Task<bool> UpdateParticipants(int id, Participant homeParticipant, Participant awayParticipant);

    Task<bool> UpdateStatus(int id, EventStatus status);

    Task<bool> UpdateOutcome(int id, MatchOutcome? outcome);

    Task<bool> UpdateOutcomeAndStatus(int id, MatchOutcome? outcome, EventStatus status);
    Task<bool> UpdateOutcomeWithCondition(int id, MatchOutcome? outcome, OutcomeCondition? condition, EventStatus? status);

    Task<bool> Update(int id, MatchInputModel inputModel);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
    
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();

}