namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;
using Exceptions;
using Models.Tournament;

public interface ITournamentsService : IDataService<Tournament>
{
    Task<IEnumerable<TournamentOutputModel>> GetAll();
    
    Task<TournamentOutputModel> Get(int id);
    
    Task<Tournament> GetInternal(int id);
    
    Task<IEnumerable<TournamentOutputModel>> Query(TournamentQuery query);
    
    ValueTask<int> Total(TournamentQuery query);
    
    /// <summary>
    /// Creates new tournament based on the input
    /// </summary>
    /// <exception cref="MissingEntryException">When provided avenueId is missing from DB.</exception>
    Task<int> Create(TournamentInputModel input, Account organiser);

    Task<bool> Update(int id, TournamentInputModel input);
    
    /// <summary>
    /// Change the avenue of a tournament
    /// </summary>
    /// <exception cref="MissingEntryException">When provided avenueId is missing from DB.</exception>
    Task<bool> ChangeAvenue(int tournamentId, int newAvenueId);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}