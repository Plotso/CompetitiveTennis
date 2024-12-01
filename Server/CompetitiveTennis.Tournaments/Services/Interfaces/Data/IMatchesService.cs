namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using CompetitiveTennis.Tournaments.Data.Models;
using Models.MatchOutcomeHandler.RatingCalculations;

public interface IMatchesService : IDataService<Match>
{
    Task<IEnumerable<MatchOutputModel>> GetAll();
    
    Task<MatchOutputModel> Get(int id);
    Task<SlimMatchOutputModel> GetForRatingCalculations(int id);

    Task<int?> GetTournamentIdForMatch(int matchId);

    Task<Match> GetInternal(int id);
    Task<MatchOutcome?> GetMatchOutcome(int id);
    Task<bool?> IsDoubles(int id);

    /// <summary>
    /// Create empty match without competitors with the goal for them to be populated on later stage
    /// </summary>
    Task<int> CreateEmptyMatch(MatchInputModel input, Tournament tournament);
    
    Task<int> Create(MatchInputModel input, Tournament tournament, Participant homeParticipant, Participant awayParticipant);

    Task<int> AddMatchFlow(int tournamentId, int matchId, int successorMatchId, bool isWinnerHome);

    Task<bool> UpdateParticipant(int id, Participant newParticipant, bool isHome);

    Task<bool> UpdateParticipants(int id, Participant homeParticipant, Participant awayParticipant);

    Task<bool> UpdateStatus(int id, EventStatus status);

    Task<bool> UpdateOutcome(int id, MatchOutcome? outcome);

    Task<bool> Update(int id, MatchInputModel inputModel);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
    
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();

}