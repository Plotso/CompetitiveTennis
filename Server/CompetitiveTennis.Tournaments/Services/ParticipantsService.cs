namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class ParticipantsService : DeletableDataService<Participant>, IParticipantsService
{
    private readonly ILogger<ParticipantsService> _logger;

    public ParticipantsService(DbContext db, ILogger<ParticipantsService> logger) : base(db)
    {
        _logger = logger;
    }

    public async Task<int> Create(string? name, int? points, Tournament tournament, Team team, bool isGuest)
    {
        var participant = new Participant
        {
            Name = name,
            IsGuest = isGuest,
            Tournament = tournament,
            Team = team,
            Points = tournament.IsLeague ? 0 : null
        };

        await SaveAsync(participant);
        return participant.Id;
    }

    public async Task<bool> AddUsersToParticipant(int id, IEnumerable<Account> users)
    {
        var participant = await All().Include(p => p.Players).SingleOrDefaultAsync(p => p.Id == id);
        if (participant == null)
            return false;

        foreach (var user in users)
        {
            var accountParticipantMap = new AccountParticipant
            {
                Account = user,
                Participant = participant
            };
            await Data.Set<AccountParticipant>().AddAsync(accountParticipantMap);
        }

        await Data.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(int id, string? name)
    {
        var participant = await All().SingleOrDefaultAsync(p => p.Id == id);
        if (participant == null)
            return false;

        participant.Name = name;
        await SaveAsync(participant);
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
        _logger.LogInformation("Participant with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }
}