namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;

public interface IParticipantsService : IDataService<Participant>
{
    Task<int> Create(string? name, int? points, Tournament tournament, Team team, bool isGuest);

    Task<bool> AddUsersToParticipant(int id, IEnumerable<Account> users);

    Task<bool> Update(int id, string name);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}