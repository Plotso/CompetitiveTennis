namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Exceptions;
using Contracts.Participant;
using CompetitiveTennis.Tournaments.Data.Models;
using Interfaces.Data;
using Microsoft.EntityFrameworkCore;

public class ParticipantsService : DeletableDataService<Participant>, IParticipantsService
{
    private readonly ILogger<ParticipantsService> _logger;

    public ParticipantsService(DbContext db, ILogger<ParticipantsService> logger) : base(db)
    {
        _logger = logger;
    }

    public async Task<Participant?> GetInternal(int id)
        => await All()
            .Include(p=> p.Players)
            .ThenInclude(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id == id);

    public IEnumerable<Participant> GetParticipantForTournament(int tournamentId)
        => All()
            .Include(p => p.Players)
            .ThenInclude(p => p.Account)
            .Where(p => p.TournamentId == tournamentId);

    public async Task<bool> IsParticipantFull(int id)
    {
        var participant = await All().Include(p => p.Players).SingleOrDefaultAsync(p => p.Id == id);
        if (participant == null)
            return false;

        return IsParticipantFull(participant.Tournament.Type, participant);
    }

    public async Task<int> Create(ParticipantInputModel input, Tournament tournament, Team? team)
    {
        if (input.IsGuest && string.IsNullOrWhiteSpace(input.Name))
            throw new InvalidInputDataException("Name is mandatory for guest participants");
        
        var participant = new Participant
        {
            Name = input.Name,
            IsGuest = input.IsGuest,
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

        if (IsParticipantFull(participant.Tournament.Type, participant))
            throw new InvalidInputDataException(
                $"No more players can be added to participant {id} since it has reached max cap for tournament type");

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

    public async Task<bool> RemoveUsersFromParticipant(int id, IEnumerable<Account> users)
    {
        var participant = await All().Include(p => p.Players).SingleOrDefaultAsync(p => p.Id == id);
        if (participant == null)
            return false;

        foreach (var user in users)
        {
            var itemToRemove = participant.Players.FirstOrDefault(p => p.AccountId == user.Id);
            if (itemToRemove == null)
                continue;
            participant.Players.Remove(itemToRemove);
        }

        await Data.SaveChangesAsync();
        //ToDo: Verify if foreach below is required
        foreach (var accountId in users.Select(u => u.Id))
        {
            var mappingEntity = await Data.Set<AccountParticipant>().Where(ap => ap.AccountId == accountId).FirstOrDefaultAsync();
            if (mappingEntity == null)
                continue;

            Data.Set<AccountParticipant>().Remove(mappingEntity);
        }
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

        var participant = await All().SingleOrDefaultAsync(t => t.Id == id);
        if (participant == null)
            return false;

        await Delete(participant, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var participant = await All().SingleOrDefaultAsync(t => t.Id == id);
        if (participant == null)
            return false;

        var accountParticipants = Data.Set<AccountParticipant>().Where(ap => ap.ParticipantId == id);
        foreach (var accountParticipant in accountParticipants)
        {
            Data.Remove(accountParticipant);
        }

        HardDelete(participant);
        _logger.LogInformation("Participant with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }

    private bool IsParticipantFull(TournamentType tournamentType, Participant participant) 
        => tournamentType switch
        {
            TournamentType.Singles => participant.IsGuest || participant.Players.Count > 0,
            TournamentType.Doubles => participant.Players.Count > 1 || participant is {IsGuest: true, Players.Count: > 0},
            _ => false
        };
}