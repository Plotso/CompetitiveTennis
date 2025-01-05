namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Contracts.MatchPeriod;

public interface IMatchPeriodInfoManager
{
    Task PersistPeriodInfoForMatch(int matchId, IEnumerable<MatchPeriodInputModel> matchPeriodInputs);
    Task DeleteMatchPeriodsForMatch(int matchId, string userId);
    Task DeleteMatchPeriodsFromSetAndGameInclusive(int matchId, string userId, int set, int game);
}