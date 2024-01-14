namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Contracts.Team;
using Data.Models;

public interface ITeamsService : IDataService<Team>
{
    Task<int> Create(string name);

    Task<Team> GetInternal(int id);

    Task<IEnumerable<TeamOutputModel>> GetAll();

    Task<TeamOutputModel> Get(int id);

    Task<bool> Update(int id, string name);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}