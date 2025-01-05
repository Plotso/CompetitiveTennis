namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod;
using Contracts.MatchPeriod.Score;

public interface IMatchPeriodsService : IDataService<MatchPeriod>
{
    Task<MatchPeriod> GetInternal(int id);
    Task<int?> GetMatchPeriodId(int matchId, int setId, int gameId);
    Task<IEnumerable<int>?> GetMatchPeriodsForMatch(int matchId);
    Task<IEnumerable<int>?> GetMatchPeriodsAfterGameAndSetInclusive(int matchId, int set, int game);
    Task<int> Create(MatchPeriodInputModel inputModel, Match match);
    Task<bool> Update(int id, MatchPeriodInputModel inputModel);
    Task<bool> Delete(int id, string userid);
    Task<bool> DeletePermanentlyForMatchId(int matchId, string userid);
    Task<bool> DeletePermanently(int id, string userid);
}