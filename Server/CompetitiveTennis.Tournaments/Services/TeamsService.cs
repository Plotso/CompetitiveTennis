namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models.Team;

public class TeamsService : DeletableDataService<Team>, ITeamsService
{
    private readonly ILogger<TeamsService> _logger;

    public TeamsService(DbContext db, ILogger<TeamsService> logger) : base(db)
    {
        _logger = logger;
    }

    public async Task<int> Create(string name)
    {
        var team = new Team {Name = name};
        await SaveAsync(team);
        return team.Id;
    }

    public async Task<Team> GetInternal(int id)
        => await All().Where(a => a.Id == id).SingleOrDefaultAsync();

    public async Task<IEnumerable<TeamOutputModel>> GetAll()
        => await All().ProjectToType<TeamOutputModel>().ToListAsync();

    public async Task<TeamOutputModel> Get(int id)
        => await All().Where(a => a.Id == id).ProjectToType<TeamOutputModel>().SingleOrDefaultAsync();

    public async Task<bool> Update(int id, string name)
    {
        var team = await All().SingleOrDefaultAsync(t => t.Id == id);
        if (team == null)
            return false;

        team.Name = name;
        await SaveAsync(team);
        return true;
    }

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var team = await All().SingleOrDefaultAsync(t => t.Id == id);
        if (team == null)
            return false;

        await Delete(team, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var team = await All().SingleOrDefaultAsync(t => t.Id == id);
        if (team == null)
            return false;

        HardDelete(team);
        _logger.LogInformation("Team with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }
}