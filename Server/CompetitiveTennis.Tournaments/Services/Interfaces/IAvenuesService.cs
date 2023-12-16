namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Contracts.Avenue;
using Data.Models;

public interface IAvenuesService : IDataService<Avenue>
{
    Task<IEnumerable<AvenueOutputModel>> GetAll();

    Task<IEnumerable<AvenueOutputModel>> Query(AvenueQuery query);

    ValueTask<int> Total(AvenueQuery query);

    Task<AvenueOutputModel> Get(int id);

    Task<Avenue> GetInternal(int id);

    Task<int> Create(AvenueInputModel input, string userId);

    Task<bool> Update(int id, AvenueInputModel input, string userId);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}