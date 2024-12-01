namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod;
using Interfaces.Data;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class MatchPeriodsService : DeletableDataService<MatchPeriod>, IMatchPeriodsService
{
    private readonly IMapper _mapper;
    private readonly ILogger<MatchPeriodsService> _logger;

    public MatchPeriodsService(DbContext db, IMapper mapper, ILogger<MatchPeriodsService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MatchPeriod> GetInternal(int id)
        => await All().SingleOrDefaultAsync(m => m.Id == id);

    public async Task<int?> GetMatchPeriodId(int matchId, int setId, int gameId)
    {
        var matchPeriod = await AllAsNoTracking().SingleOrDefaultAsync(mp => mp.MatchId == matchId && mp.Set == setId && mp.Game == gameId);
        return matchPeriod?.Id;
    }

    public async Task<IEnumerable<int>?> GetMatchPeriodsForMatch(int matchId)
    {
        var matchPeriods = await AllAsNoTracking().Where(mp => mp.MatchId == matchId).ToArrayAsync();
        return matchPeriods.Any() ? matchPeriods.Select(mp => mp.Id) : null;
    }

    public async Task<IEnumerable<int>?> GetMatchPeriodsAfterGameAndSetInclusive(int matchId, int set, int game)
    {
        var matchPeriods = await AllAsNoTracking()
            .Where(mp => mp.MatchId == matchId && ((mp.Set == set && mp.Game >= game) || mp.Set > set))// IsApplicablePeriodAfter(set, game, mp))
            .ToArrayAsync();
        return matchPeriods.Any() ? matchPeriods.Select(mp => mp.Id) : null;
    }

    private static bool IsApplicablePeriodAfter(int setId, int gameId, MatchPeriod mp) => IsBiggerGameInSetInclusive(setId, gameId, mp) || IsBiggerSet(setId, mp);

    private static bool IsBiggerGameInSetInclusive(int setId, int gameId, MatchPeriod mp) => mp.Set == setId && mp.Game >= gameId;

    private static bool IsBiggerSet(int setId, MatchPeriod mp) => mp.Set > setId;

    public async Task<int> Create(MatchPeriodInputModel inputModel, Match match)
    {
        var matchPeriod = _mapper.Map<MatchPeriod>(inputModel);
        matchPeriod.Match = match;

        await SaveAsync(matchPeriod);
        return matchPeriod.Id;
    }

    public async Task<bool> Update(int id, MatchPeriodInputModel inputModel)
    {
        var matchPeriod = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (matchPeriod == null)
            return false;

        matchPeriod.Set = inputModel.Set;
        matchPeriod.Game = inputModel.Game;
        matchPeriod.Winner = inputModel.Winner;
        matchPeriod.Status = inputModel.Status;
        matchPeriod.Server = inputModel.Server;
        matchPeriod.IsTiebreak = inputModel.IsTiebreak;

        await SaveAsync(matchPeriod);
        return true;
    }
    

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var matchPeriod = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (matchPeriod == null)
            return false;

        await Delete(matchPeriod, userid);
        return true;
    }

    public async Task<bool> DeletePermanentlyForMatchId(int matchId, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var matchPeriods = await All().Where(s => s.MatchId == matchId).ToListAsync();
        if (matchPeriods == null || matchPeriods.Count == 0)
            return false;

        foreach (var matchPeriod in matchPeriods)
        {
            HardDelete(matchPeriod);
            _logger.LogInformation("MatchPeriod with id: {Id} has been permanently deleted by UserId: {Userid}", matchPeriod.Id, userid);
        }
        await Data.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var matchPeriod = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (matchPeriod == null)
            return false;

        HardDelete(matchPeriod);
        _logger.LogInformation("MatchPeriod with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }
}