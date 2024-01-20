namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Contracts.Score;
using Data.Models;
using Interfaces;
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

    public async Task<int> Create(ScoreInputModel inputModel, Match match)
    {
        var score = _mapper.Map<Score>(inputModel);
        score.Match = match;

        await SaveAsync(score);
        return score.Id;
    }

    public async Task<bool> Update(int id, ScoreInputModel inputModel)
    {
        var score = await All().SingleOrDefaultAsync(s => s.Id == id);
        if (score == null)
            return false;

        score.Set = inputModel.Set;
        score.Game = inputModel.Game;
        score.Participant1Points = inputModel.Participant1Points;
        score.Participant2Points = inputModel.Participant2Points;
        score.Status = inputModel.Status;
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