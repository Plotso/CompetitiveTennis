namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;
using Models.Participant;

public interface IParticipantsService : IDataService<Participant>
{
    Task<Participant?> GetInternal(int id);
    
    Task<int> Create(ParticipantInputModel input, Tournament tournament, Team? team);

    Task<bool> AddUsersToParticipant(int id, IEnumerable<Account> users);

    Task<bool> RemoveUsersFromParticipant(int id, IEnumerable<Account> users);

    Task<bool> Update(int id, string name);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}