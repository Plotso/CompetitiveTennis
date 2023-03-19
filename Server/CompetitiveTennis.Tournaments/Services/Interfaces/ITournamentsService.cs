namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;
using Exceptions;
using Models.Tournament;

public interface ITournamentsService : IDataService<Tournament>
{
    Task<bool> RemoveParticipant(int id, Participant participant);
    
    Task<IEnumerable<TournamentOutputModel>> GetAll();
    
    Task<TournamentOutputModel> Get(int id);
    
    Task<Tournament> GetInternal(int id);
    
    Task<string> GetOrganiserUserId(int id);
    
    Task<IEnumerable<TournamentOutputModel>> Query(TournamentQuery query);
    
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