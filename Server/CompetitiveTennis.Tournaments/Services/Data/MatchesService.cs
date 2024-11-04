namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Interfaces.Data;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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
        => _mapper.Map<MatchOutputModel>(await All()
            .Where(a => a.Id == id)
            .EnrichWithParticipants()
            .Include(m => m.Periods)
            .ThenInclude(mp => mp.Scores)
            .SingleOrDefaultAsync());

    public async Task<MatchOutputModel> GetOLD(int id)
        => await All()
            .Where(m => m.Id == id)
            .Include(m => m.Participants)
            .Include(m => m.Periods)
            .ThenInclude(mp => mp.Scores)
            .ProjectToType<MatchOutputModel>()
            .SingleOrDefaultAsync();

    public async Task<int?> GetTournamentIdForMatch(int matchId) 
        => await All()
            .Where(m => m.Id == matchId)
            .Select(m => m.TournamentId)
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

    public async Task<int> Create(MatchInputModel input, Tournament tournament, Participant homeParticipant, Participant awayParticipant)
    {
        var match = _mapper.Map<Match>(input);
        match.Tournament = tournament;

        await SaveAsync(match);

        await AddParticipant(match, homeParticipant, DataConstants.ParticipantSpecifiers.Home);
        await AddParticipant(match, awayParticipant, DataConstants.ParticipantSpecifiers.Away);
        await Data.SaveChangesAsync();
        return match.Id;
    }

    public async Task<int> AddMatchFlow(int tournamentId, int matchId, int successorMatchId, bool isWinnerHome)
    {
        var match = await All().Include(m => m.Tournament).SingleOrDefaultAsync(m => m.Id == matchId);
        if (match == null)
            return ServiceConstants.MissingEntityId;
        if (match.TournamentId != tournamentId)
            throw new ArgumentException($"Match {matchId} is linked to a different tournament than the one from the input");
        
        var successorMatch = await All().Include(m => m.Tournament).SingleOrDefaultAsync(m => m.Id == successorMatchId);
        if (successorMatch == null)
            return ServiceConstants.MissingEntityId;
        if (successorMatch.TournamentId != match.TournamentId)
            throw new ArgumentException($"Match {matchId} and followup match {successorMatch} are linked to different tournaments");

        var matchFlow = new MatchFlow
        {
            IsHome = isWinnerHome,
            MatchId = match.Id,
            SuccessorMatchId = successorMatch.Id,
            Tournament = match.Tournament
        };
        await Data.Set<MatchFlow>().AddAsync(matchFlow);
        await Data.SaveChangesAsync();
        return matchFlow.Id;
    }

    public async Task<bool> UpdateParticipant(int id, Participant newParticipant, bool isHome)
    {
        var match = await All().Include(m => m.Participants).SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        var specifier = isHome ? DataConstants.ParticipantSpecifiers.Home : DataConstants.ParticipantSpecifiers.Away;
        var currentParticipant = match.Participants.FirstOrDefault(p => p.Specifier == specifier);
        if (currentParticipant != null)
        {
            match.Participants.Remove(currentParticipant);
        }
        await AddParticipant(match, newParticipant, specifier);

        await Data.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateParticipants(int id, Participant homeParticipant, Participant awayParticipant)
    {
        var match = await All().Include(m => m.Participants).SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;
        
        var hasHomeParticipant = match.Participants.Select(p => p.ParticipantId).Any(pid => pid == homeParticipant.Id);
        var hasAwayParticipant = match.Participants.Select(p => p.ParticipantId).Any(pid => pid == awayParticipant.Id);

        if (!hasHomeParticipant && hasAwayParticipant)
        {
            var participantsToRemove = match.Participants.Where(p => p.ParticipantId != awayParticipant.Id);
            foreach (var participantToRemove in participantsToRemove)
            {
                match.Participants.Remove(participantToRemove);
            }
            await AddParticipant(match, homeParticipant, DataConstants.ParticipantSpecifiers.Home);
        }

        if (!hasAwayParticipant && hasHomeParticipant)
        {
            var participantsToRemove = match.Participants.Where(p => p.ParticipantId != homeParticipant.Id);
            foreach (var participantToRemove in participantsToRemove)
            {
                match.Participants.Remove(participantToRemove);
            }
            await AddParticipant(match, awayParticipant, DataConstants.ParticipantSpecifiers.Away);
        }

        if (!hasAwayParticipant && !hasHomeParticipant)
        {
            var participantsToRemove = match.Participants.Select(p => p.ParticipantId);
            foreach (var participantId in participantsToRemove)
            {
                var participant = match.Participants.FirstOrDefault(p => p.ParticipantId == participantId);
                match.Participants.Remove(participant);
            }
            await AddParticipant(match, homeParticipant, DataConstants.ParticipantSpecifiers.Home);
            await AddParticipant(match, awayParticipant, DataConstants.ParticipantSpecifiers.Away);
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

    public async Task BeginTransaction() => await Data.Database.BeginTransactionAsync();
    public async Task CommitTransaction() => await Data.Database.CommitTransactionAsync();
    public async Task RollbackTransaction() => await Data.Database.RollbackTransactionAsync();

    private async Task AddParticipant(Match match, Participant participant, string specifier)
    {
        var entities = new ParticipantMatch
        {
            Match = match,
            Participant = participant,
            Specifier = specifier
        };
        await Data.Set<ParticipantMatch>().AddAsync(entities);
    }
}