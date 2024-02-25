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
        => await All().Where(m => m.Id == id).SingleOrDefaultAsync();

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