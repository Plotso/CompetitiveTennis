namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Contracts.Score;
using Data.Models;

public interface IScoresService : IDataService<Score>
{
    Task<int> Create(ScoreInputModel inputModel, Match match);
    Task<bool> Update(int id, ScoreInputModel inputModel);
    Task<bool> Delete(int id, string userid);
    Task<bool> DeletePermanently(int id, string userid);
}