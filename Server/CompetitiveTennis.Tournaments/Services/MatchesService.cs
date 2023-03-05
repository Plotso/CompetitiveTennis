namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Data.Models.Enums;
using Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models.Match;

public class MatchesService : DeletableDataService<Match>, IMatchesService
{
    private readonly IMapper _mapper;
    private readonly ILogger<MatchesService> _logger;

    public MatchesService(DbContext db, IMapper mapper, ILogger<MatchesService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MatchOutputModel>> GetAll()
        => await All()
            //.Include(m => m.Participants)  ToDo: Verify if Include is needed
            .ProjectToType<MatchOutputModel>().ToListAsync();

    public async Task<MatchOutputModel> Get(int id)
        => await All()
            .Where(m => m.Id == id)
            //.Include(m => m.Scores)  ToDo: Verify if Include is needed
            //.Include(m => m.Participants)
            .ProjectToType<MatchOutputModel>()
            .SingleOrDefaultAsync();

    public async Task<Match> GetInternal(int id)
        => await All().Where(m => m.Id == id).SingleOrDefaultAsync();

    public async Task<int> CreateEmptyMatch(MatchInputModel input, Tournament tournament)
    {
        var match = _mapper.Map<Match>(input);
        match.Tournament = tournament;

        await SaveAsync(match);
        return match.Id;
    }

    public async Task<int> Create(MatchInputModel input, Tournament tournament, Participant participant1, Participant participant2)
    {
        var match = _mapper.Map<Match>(input);
        match.Tournament = tournament;
        match.Participant1Id = participant1.Id;

        await SaveAsync(match);

        await AddParticipants(match, participant1, participant2);
        await Data.SaveChangesAsync();
        return match.Id;
    }

    public async Task<bool> UpdateParticipants(int id, Participant participant1, Participant participant2)
    {
        var match = await All().Include(m => m.Participants).SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;
        
        match.Participant1Id = participant1.Id;
        var hasParticipant1 = match.Participants.Select(p => p.ParticipantId).Any(pid => pid == participant1.Id);
        var hasParticipant2 = match.Participants.Select(p => p.ParticipantId).Any(pid => pid == participant2.Id);

        if (!hasParticipant1 && hasParticipant2)
        {
            var participantsToRemove = match.Participants.Where(p => p.ParticipantId != participant2.Id);
            foreach (var participantToRemove in participantsToRemove)
            {
                match.Participants.Remove(participantToRemove);
            }
            await AddParticipants(match, participant1);
        }

        if (!hasParticipant2 && hasParticipant1)
        {
            var participantsToRemove = match.Participants.Where(p => p.ParticipantId != participant1.Id);
            foreach (var participantToRemove in participantsToRemove)
            {
                match.Participants.Remove(participantToRemove);
            }
            await AddParticipants(match, participant2);
        }

        if (!hasParticipant2 && !hasParticipant1)
        {
            var participantsToRemove = match.Participants.Select(p => p.ParticipantId);
            foreach (var participantId in participantsToRemove)
            {
                var participant = match.Participants.FirstOrDefault(p => p.ParticipantId == participantId);
                match.Participants.Remove(participant);
            }
            await AddParticipants(match, participant1, participant2);
        }

        await Data.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatus(int id, EventStatus status)
    {
        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        match.Status = status;
        await SaveAsync(match);
        return true;
    }

    public async Task<bool> UpdateOutcome(int id, MatchOutcome? outcome)
    {
        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        match.Outcome = outcome;
        await SaveAsync(match);
        return true;
    }

    public async Task<bool> Update(int id, MatchInputModel inputModel)
    {
        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        match.StartDate = inputModel.StartDate;
        match.EndDate = inputModel.EndDate;
        match.MatchWonPoints = inputModel.MatchWonPoints;
        match.SetWonPoints = inputModel.SetWonPoints;
        match.GameWonPoints = inputModel.GameWonPoints;
        match.Stage = inputModel.Stage;
        match.Details = inputModel.Details;
        match.NextMatchId = inputModel.NextMatchId;

        await SaveAsync(match);
        return true;
    }

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        await Delete(match, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;

        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        HardDelete(match);
        _logger.LogInformation("Match with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }

    private async Task AddParticipants(Match match, params Participant[] participants)
    {
        var entities = participants.Select(p => new ParticipantMatch
        {
            Match = match,
            Participant = p
        });
        await Data.Set<ParticipantMatch>().AddRangeAsync(entities);
    }
}