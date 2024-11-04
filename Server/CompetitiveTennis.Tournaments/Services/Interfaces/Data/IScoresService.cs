namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod.Score;

public interface IScoresService : IDataService<Score>
{
    Task<bool> HasScoreForMatchPeriod(int matchPeriodId, int periodPointNumber);
    Task<int> Create(ScoreInputModel inputModel, MatchPeriod matchPeriod);
    Task<bool> Update(int id, ScoreInputModel inputModel);
    Task<bool> Delete(int id, string userid);
    Task<bool> DeletePermanentlyForMatchPeriodId(int matchPeriodId, string userid);
    Task<bool> DeletePermanently(int id, string userid);
}