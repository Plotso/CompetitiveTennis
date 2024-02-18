namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod;
using Contracts.MatchPeriod.Score;
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

    public async Task<int> Create(MatchPeriodInputModel inputModel, Match match)
    {
        var matchPeriod = _mapper.Map<MatchPeriod>(inputModel);
        matchPeriod.Match = match;

        await SaveAsync(matchPeriod);
        return matchPeriod.Id;
    }

    public async Task<bool> Update(int id, MatchPeriodInputModel inputModel)
    {
        var score = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (score == null)
            return false;

        score.Set = inputModel.Set;
        score.Game = inputModel.Game;
        score.Winner = inputModel.Winner;
        score.Status = inputModel.Status;
        score.Server = inputModel.Server;

        await SaveAsync(score);
        return true;
    }

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var score = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (score == null)
            return false;

        await Delete(score, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var score = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (score == null)
            return false;

        HardDelete(score);
        _logger.LogInformation("MatchPeriod with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }
}