﻿namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Extensions;
using Interfaces.Data;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.MatchOutcomeHandler.RatingCalculations;

public class MatchesService : DeletableDataService<Match>, IMatchesService
{
    private readonly IMapper _mapper;
    private readonly ILogger<MatchesService> _logger;

    public MatchesService(DbContext db, IMapper mapper, ILogger<MatchesService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MatchOutputModel>> Query(MatchQuery query)
        => _mapper.Map<IEnumerable<MatchOutputModel>>(await GetTournamentsQuery(query).PageFilterResult(query)
            .ToListAsync());

    public async Task<int> Total(MatchQuery query) => await GetTournamentsQuery(query).CountAsync();

    public async Task<IEnumerable<MatchOutputModel>> GetAll()
        => await All()
            //.Include(m => m.Participants)  ToDo: Verify if Include is needed
            .ProjectToType<MatchOutputModel>().ToListAsync();

    public async Task<MatchOutputModel> Get(int id)
        => _mapper.Map<MatchOutputModel>(await AllAsNoTracking()
            .Where(a => a.Id == id)
            .EnrichWithParticipants()
            .Include(m => m.Periods)
            .ThenInclude(mp => mp.Scores)
            .SingleOrDefaultAsync());

    public async Task<SlimMatchOutputModel> GetForRatingCalculations(int id)
        => _mapper.Map<SlimMatchOutputModel>(await AllAsNoTracking()
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

    public async Task<IEnumerable<MatchParticipantRatingInfo>> GetMatchParticipantsInfo(int matchId)
    {
        var match = await AllAsNoTracking()
            .Where(m => m.Id == matchId)
            .Include(m => m.Participants)
            .SingleOrDefaultAsync();
        if (match == null || match.Participants.IsNullOrEmpty())
            return Array.Empty<MatchParticipantRatingInfo>();

        return match.Participants.Select(mp => new MatchParticipantRatingInfo(match.Id, mp.ParticipantId, mp.PrematchRating, mp.Specifier));
    }

    /// <summary>
    /// Check whether match status is != NotStarted or whether it has any period scores
    /// </summary>
    public async Task<bool?> HasMatchStarted(int id)
    {
        var match = await AllAsNoTracking()
            .Where(m => m.Id == id)
            .Include(m => m.Periods)
            .SingleOrDefaultAsync();
        return match != null && (match.Status != EventStatus.NotStarted || match.Periods.Any());
    }

    public async Task<bool?> HasMatchEnded(int id)
    {
        var match = await AllAsNoTracking()
            .Where(m => m.Id == id)
            .Include(m => m.Periods)
            .SingleOrDefaultAsync();
        return match != null && (match.Status == EventStatus.Ended);
    }

    public async Task<MatchOutcome?> GetMatchOutcome(int id)
        => await AllAsNoTracking().Where(m => m.Id == id).Select(m => m.Outcome).FirstOrDefaultAsync();

    public async Task<bool?> IsDoubles(int id)
    {
        var matchTournamentTypeInfo = await AllAsNoTracking().Where(m => m.Id == id).Include(m => m.Tournament).Select(m => new {m.Id, m.Tournament.Type}).FirstOrDefaultAsync();
        return matchTournamentTypeInfo?.Type == TournamentType.Doubles;
    }

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
    
    public async Task<MatchFlow?> GetMatchFlow(int matchId) 
        => await Data.Set<MatchFlow>().AsNoTracking().Where(m => m.MatchId == matchId).FirstOrDefaultAsync();

    public async Task<bool> UpdateParticipant(int id, Participant newParticipant, bool isHome)
    {
        var match = await All()
            .Include(m => m.Tournament)
            .Include(m => m.Participants)
            .SingleOrDefaultAsync(m => m.Id == id);
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
        var match = await All()
            .Include(m => m.Tournament)
            .Include(m => m.Participants)
            .SingleOrDefaultAsync(m => m.Id == id);
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

    public async Task<bool> UpdateOutcomeAndStatus(int id, MatchOutcome? outcome, EventStatus status)
    {
        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        match.Outcome = outcome;
        match.Status = status;
        await SaveAsync(match);
        return true;
    }

    public async Task<bool> UpdateOutcomeWithCondition(int id, MatchOutcome? outcome, OutcomeCondition? condition, EventStatus? status)
    {
        var match = await All().SingleOrDefaultAsync(m => m.Id == id);
        if (match == null)
            return false;

        match.Outcome = outcome;
        match.OutcomeCondition = condition;
        if (status != null)
            match.Status = status.Value;
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
        var prematchRating = match.Tournament.Type == TournamentType.Doubles ?
            participant.Players.Sum(p => p.Account.DoublesRating) :  
            participant.Players.Sum(p => p.Account.PlayerRating);

        if (prematchRating == 0)
            prematchRating = 1000;
            
        var entities = new ParticipantMatch
        {
            Match = match,
            Participant = participant,
            Specifier = specifier,
            PrematchRating = prematchRating
        };
        await Data.Set<ParticipantMatch>().AddAsync(entities);
    }
    
    private IQueryable<Match> GetTournamentsQuery(MatchQuery query, int? matchId = null)
    {
        var dataQuery = All().EnrichMatchQueryData();

        if (matchId.HasValue)
        {
            dataQuery = dataQuery.Where( t=> t.Id == matchId);
            return dataQuery;
        }

        if (!string.IsNullOrWhiteSpace(query.ParticipantUsername) && !query.IsParticipantWinner.HasValue)
            dataQuery = dataQuery.Where(m => m.Participants.Any(p => p.Participant.Players.Any(pp => pp.Account.Username == query.ParticipantUsername)));
        // ToDo: Cache the results of the below filtered query somewhere since it's very heavy
        if (!string.IsNullOrWhiteSpace(query.ParticipantUsername) && query.IsParticipantWinner.HasValue && query.IsParticipantWinner.Value)
            dataQuery = dataQuery.Where(m => m.Status == EventStatus.Ended &&
                (m.Outcome == MatchOutcome.ParticipantOne &&
                 m.Participants.Any(p =>
                     p.Specifier == DataConstants.ParticipantSpecifiers.Home &&
                     p.Participant.Players.Any(pp => pp.Account.Username == query.ParticipantUsername))) ||
                (m.Outcome == MatchOutcome.ParticipantTwo &&
                 m.Participants.Any(p =>
                     p.Specifier == DataConstants.ParticipantSpecifiers.Away &&
                     p.Participant.Players.Any(pp => pp.Account.Username == query.ParticipantUsername))));

        if (!string.IsNullOrWhiteSpace(query.ParticipantName))
            dataQuery = dataQuery.Where(m => m.Participants.Any(p => string.Equals(p.Participant.Name, query.ParticipantName, StringComparison.OrdinalIgnoreCase) ||
                                                                     p.Participant.Players.Any(pp => AccountContainsName(pp.Account, query.ParticipantName))));
        if (query.Status.HasValue)
            dataQuery = dataQuery.Where(m => m.Status == query.Status.Value);
        if (query.OutcomeCondition.HasValue)
            dataQuery = dataQuery.Where(m => m.OutcomeCondition == query.OutcomeCondition.Value);
        if (query.Surface.HasValue)
            dataQuery = dataQuery.Where(m => m.Tournament.Surface == query.Surface);
        if (query.TournamentType.HasValue)
            dataQuery = dataQuery.Where(m => m.Tournament.Type == query.TournamentType);
        if (query.TournamentStage.HasValue)
            dataQuery = dataQuery.Where(m => m.Stage == query.TournamentStage.Value);
        if (query.DateRangeFrom.HasValue)
            dataQuery = dataQuery.Where(m => m.StartDate >= query.DateRangeFrom);
        if (query.DateRangeUntil.HasValue)
            dataQuery = dataQuery.Where(m => m.EndDate <= query.DateRangeUntil);

        dataQuery = dataQuery.SortQuery(query.SortOptions);

        return dataQuery;
    }

    private bool AccountContainsName(Account account, string name)
        => string.Equals(account.FirstName, name, StringComparison.OrdinalIgnoreCase) ||
           string.Equals(account.LastName, name, StringComparison.OrdinalIgnoreCase) ||
           string.Equals($"{account.FirstName} {account.LastName}", name, StringComparison.OrdinalIgnoreCase);
}