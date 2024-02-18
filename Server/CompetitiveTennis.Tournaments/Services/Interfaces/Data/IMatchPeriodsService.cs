namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod;
using Contracts.MatchPeriod.Score;

public interface IMatchPeriodsService : IDataService<MatchPeriod>
{
    Task<int> Create(MatchPeriodInputModel inputModel, Match match);
    Task<bool> Update(int id, MatchPeriodInputModel inputModel);
    Task<bool> Delete(int id, string userid);
    Task<bool> DeletePermanently(int id, string userid);
}