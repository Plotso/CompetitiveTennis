﻿namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;
using Models.Avenue;

public interface IAvenuesService : IDataService<Avenue>
{
    Task<IEnumerable<AvenueOutputModel>> GetAll();

    Task<IEnumerable<AvenueOutputModel>> Query(AvenueQuery query);

    ValueTask<int> Total(AvenueQuery query);

    Task<AvenueOutputModel> Get(int id);

    Task<int> Create(AvenueInputModel input);

    Task<bool> Update(int id, AvenueInputModel input);

    Task<bool> Delete(int id, string userid);

    Task<bool> DeletePermanently(int id, string userid);
}