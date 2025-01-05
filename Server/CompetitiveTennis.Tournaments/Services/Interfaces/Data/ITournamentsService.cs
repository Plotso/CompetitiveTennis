namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using Exceptions;
using Contracts.Tournament;
using CompetitiveTennis.Tournaments.Data.Models;
using Models;

public interface ITournamentsService : IDataService<Tournament>
{
    Task<bool> RemoveParticipant(int id, Participant participant);
    
    Task<IEnumerable<FullTournamentOutputModel>> GetAll();

    Task<bool> IsAccountPresentInAnyParticipant(int accountId, int tournamentId);

    Task<bool> HasTournamentAlreadyStarted(int tournamentId);
    
    Task<FullTournamentOutputModel> Get(int id);
    Task<TournamentMatchFlowInfo> GetTournamentMatchFlowInfo(int id);
    Task<string> GetTournamentName(int id);
    Task<string> GetOrganiserUsername(int id);
    Task<FullTournamentOutputModel> GetForDrawGeneration(int id);
    
    Task<Tournament> GetInternal(int id);
    
    Task<string> GetOrganiserUserId(int id);

    Task<IEnumerable<int>> GetRegisteredAccountsForTournament(int tournamentId);
    
    Task<IEnumerable<FullTournamentOutputModel>> Query(TournamentQuery query);
    
    ValueTask<int> Total(TournamentQuery query);
    
    Task<int> Create(TournamentInputModel input, Account organiser, Avenue avenue);

    Task<bool> Update(int id, TournamentInputModel input);
    
    /// <summary>
    /// Change the avenue of a tournament
    /// </summary>
    /// <exception cref="MissingEntryException">When provided avenueId is missing from DB.</exception>
    Task<bool> ChangeAvenue(int tournamentId, int newAvenueId);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}