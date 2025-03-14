﻿namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Contracts.MatchPeriod.Score;
using Interfaces.Data;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class ScoresService : DeletableDataService<Score>, IScoresService
{
    private readonly IMapper _mapper;
    private readonly ILogger<ScoresService> _logger;

    public ScoresService(DbContext db, IMapper mapper, ILogger<ScoresService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> HasScoreForMatchPeriod(int matchPeriodId, int periodPointNumber) 
        => await All().AnyAsync(s => s.MatchPeriodId == matchPeriodId && s.PeriodPointNumber == periodPointNumber);

    public async Task<int> Create(ScoreInputModel inputModel, MatchPeriod matchPeriod)
    {
        var score = _mapper.Map<Score>(inputModel);
        score.MatchPeriod = matchPeriod;

        await SaveAsync(score);
        return score.Id;
    }

    public async Task<bool> Update(int id, ScoreInputModel inputModel)
    {
        var score = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (score == null)
            return false;

        score.PeriodPointNumber = inputModel.PeriodPointNumber;
        score.Participant1Points = inputModel.Participant1Points;
        score.Participant2Points = inputModel.Participant2Points;
        score.PointWinner = inputModel.PointWinner;

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

    public async Task<bool> DeletePermanentlyForMatchPeriodId(int matchPeriodId, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var scores = await All().Where(s => s.MatchPeriodId == matchPeriodId).ToListAsync();
        if (scores == null || scores.Count == 0)
            return false;

        foreach (var score in scores)
        {
            HardDelete(score);
            _logger.LogInformation("Score with id: {Id} has been permanently deleted by UserId: {Userid}", score.Id, userid);
        }
        await Data.SaveChangesAsync();
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
        _logger.LogInformation("Score with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }
}