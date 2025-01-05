namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using Contracts.Participant;
using CompetitiveTennis.Tournaments.Data.Models;

public interface IParticipantsService : IDataService<Participant>
{
    Task<Participant?> GetInternal(int id);
    IEnumerable<Participant> GetParticipantForTournament(int tournamentId);

    Task<bool> IsParticipantFull(int id);
    
    Task<int> Create(ParticipantInputModel input, Tournament tournament, Team? team);

    Task<bool> AddUsersToParticipant(int id, IEnumerable<Account> users);

    Task<bool> RemoveUsersFromParticipant(int id, IEnumerable<Account> users);

    Task<bool> Update(int id, string name);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}