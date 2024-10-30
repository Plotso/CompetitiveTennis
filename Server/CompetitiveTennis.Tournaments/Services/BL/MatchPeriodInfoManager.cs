namespace CompetitiveTennis.Tournaments.Services.BL;

using Contracts.MatchPeriod;
using Interfaces.BL;
using Interfaces.Data;
using Tournaments.Data.Models;

public class MatchPeriodInfoManager : IMatchPeriodInfoManager
{
    private readonly IMatchPeriodsService _matchPeriodsService;
    private readonly IMatchesService _matchesService;
    private readonly IScoresService _scoresService;
    private readonly ILogger<MatchPeriodInfoManager> _logger;

    public MatchPeriodInfoManager(
        IMatchPeriodsService matchPeriodsService,
        IMatchesService matchesService,
        IScoresService scoresService,
        ILogger<MatchPeriodInfoManager> logger)
    {
        _matchPeriodsService = matchPeriodsService;
        _matchesService = matchesService;
        _scoresService = scoresService;
        _logger = logger;
    }

    public async Task PersistPeriodInfoForMatch(int matchId, IEnumerable<MatchPeriodInputModel> matchPeriodInputs)
    {
        var match = await _matchesService.GetInternal(matchId);
        foreach (var matchPeriodInput in matchPeriodInputs)
            await PersistMatchPeriod(match, matchPeriodInput);
    }

    public async Task DeleteMatchPeriodsForMatch(int matchId, string userId)
    {
        var matchPeriodIdsForMatch = await _matchPeriodsService.GetMatchPeriodsForMatch(matchId);
        if (matchPeriodIdsForMatch == null)
            return;
        foreach (var matchPeriodId in matchPeriodIdsForMatch)
            await _scoresService.DeletePermanentlyForMatchPeriodId(matchPeriodId, userId);
        await _matchPeriodsService.DeletePermanentlyForMatchId(matchId, userId);
    }

    public async Task DeleteMatchPeriodsFromSetAndGameInclusive(int matchId, string userId, int set, int game)
    {
        var matchPeriodIdsForMatch = await _matchPeriodsService.GetMatchPeriodsAfterGameAndSetInclusive(matchId, set, game);
        if (matchPeriodIdsForMatch == null)
            return;
        foreach (var matchPeriodId in matchPeriodIdsForMatch)
        {
            await _scoresService.DeletePermanentlyForMatchPeriodId(matchPeriodId, userId);
            await _matchPeriodsService.DeletePermanently(matchPeriodId, userId);
        }
    }

    private async Task PersistMatchPeriod(Match match, MatchPeriodInputModel matchPeriodInput)
    {
        var matchPeriodId = await _matchPeriodsService.GetMatchPeriodId(match.Id, matchPeriodInput.Set, matchPeriodInput.Game);
        if (matchPeriodId is null)
            matchPeriodId = await _matchPeriodsService.Create(matchPeriodInput, match);
            
        var matchPeriod = await _matchPeriodsService.GetInternal(matchPeriodId!.Value);
        foreach (var score in matchPeriodInput.Scores)
            await _scoresService.Create(score, matchPeriod);
    }
}