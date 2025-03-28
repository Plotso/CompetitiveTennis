namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using Exceptions;
using Contracts.Tournament;
using CompetitiveTennis.Tournaments.Data;
using CompetitiveTennis.Tournaments.Data.Models;
using Extensions;
using Models;
using Interfaces.Data;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class TournamentsService : DeletableDataService<Tournament>, ITournamentsService
{
    private readonly IMapper _mapper;
    private readonly ILogger<TournamentsService> _logger;

    public TournamentsService(DbContext db, IMapper mapper, ILogger<TournamentsService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> RemoveParticipant(int id, Participant participant)
    {
        var tournament = await Data.FindAsync<Tournament>(id);
        if (tournament == null)
            return false;
        //ToDo: Verify all flows are handled without errors and with expected behaviour
        if (tournament.Participants.Any(p => p.Id == participant.Id))
        {
            tournament.Participants.Remove(participant);
            await SaveAsync(tournament);
        }
        return true;
    }

    public async Task<IEnumerable<FullTournamentOutputModel>> GetAll()
        => _mapper.Map<IEnumerable<FullTournamentOutputModel>>(await All()
            .EnrichTournamentQueryData()
            .Include(t => t.Matches) //Only matches are needed for FE visualisation purposes, not whole EnrichWithMatches data method
            .ToListAsync());

    public async Task<bool> HasTournamentAlreadyStarted(int tournamentId)
    {
        var tournament = await GetInternal(tournamentId);
        return tournament != null && tournament.Matches.Any();
    }

    public async Task<FullTournamentOutputModel> Get(int id)
        => _mapper.Map<FullTournamentOutputModel>(await All()
            .Where(a => a.Id == id)
            .EnrichTournamentQueryData()
            .EnrichWithMatches()
            .SingleOrDefaultAsync());

    public async Task<TournamentMatchFlowInfo> GetTournamentMatchFlowInfo(int id)
        => _mapper.Map<TournamentMatchFlowInfo>(await All()
            .Where(t => t.Id == id)
            .Include(t => t.MatchFlows)
            .Select(t => new {t.Id, t.MatchFlows})
            .SingleOrDefaultAsync());

    public async Task<string> GetTournamentName(int id)
        => await AllAsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => t.Title)
            .FirstOrDefaultAsync();

    public async Task<string> GetOrganiserUsername(int id) 
        => await AllAsNoTracking()
            .Include(t => t.Organiser)
            .Where(t => t.Id == id)
            .Select(t => t.Organiser.Username)
            .FirstOrDefaultAsync();


    public async Task<bool> IsAccountPresentInAnyParticipant(int accountId, int tournamentId)
    {
        var tournament = await AllAsNoTracking()
            .Include(t => t.Participants)
            .ThenInclude(tp => tp.Players)
            .FirstOrDefaultAsync(t => t.Id == tournamentId);
        if (tournament == null)
            return false;

        return tournament.Participants.Any(tp => tp.Players.Any(p => p.AccountId == accountId));
        var participatingAccountIds = tournament.Participants.SelectMany(p => p.Players).Select(ap => ap.AccountId);
        return participatingAccountIds.Contains(accountId);
    }

    public async Task<FullTournamentOutputModel> GetForDrawGeneration(int id)
        => _mapper.Map<FullTournamentOutputModel>(await All()
            .Where(a => a.Id == id)
            .EnrichTournamentQueryForDrawGeneration()
            .EnrichWithMatches()
            .SingleOrDefaultAsync());

    public async Task<Tournament> GetInternal(int id)
        => await All().Where(a => a.Id == id).Include(t => t.Matches).SingleOrDefaultAsync();

    public async Task<string> GetOrganiserUserId(int id)
        => await All()
            .Where(a => a.Id == id)
            .Include(a => a.Organiser)
            .Select(t => t.Organiser.UserId)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<int>> GetRegisteredAccountsForTournament(int tournamentId)
        => await AllAsNoTracking()
            .Include(t => t.Participants)
            .ThenInclude(p => p.Players)
            .Where(t => t.Id == tournamentId)
            .SelectMany(t => t.Participants.SelectMany(p => p.Players.Select(a => a.AccountId)))
            .ToListAsync();

    public async Task<IEnumerable<FullTournamentOutputModel>> Query(TournamentQuery query)
        => _mapper.Map<IEnumerable<FullTournamentOutputModel>>(await GetTournamentsQuery(query).PageFilterResult(query)
            .ToListAsync());

    public async Task<int> Total(TournamentQuery query) => await GetTournamentsQuery(query).CountAsync();

    public async Task<int> Create(TournamentInputModel input, Account organiser, Avenue avenue)
    {
        var tournament = _mapper.Map<Tournament>(input);
        tournament.Avenue = avenue;
        tournament.Organiser = organiser;

        await SaveAsync(tournament);
        return tournament.Id;
    }

    public async Task<bool> Update(int id, TournamentInputModel input)
    {
        var tournament = await Data.FindAsync<Tournament>(id);
        if (tournament == null)
            return false;
        tournament.Title = input.Title;
        tournament.Rules = input.Rules;
        tournament.Description = input.Description;
        tournament.Type = input.Type;
        tournament.Surface = input.Surface;
        tournament.EntryFee = input.EntryFee;
        tournament.Prize = input.Prize;
        tournament.CourtsAvailable = input.CourtsAvailable;
        tournament.MinParticipants = input.MinParticipants;
        tournament.MaxParticipants = input.MaxParticipants;
        tournament.MatchWonPoints = input.MatchWonPoints;
        tournament.SetWonPoints = input.SetWonPoints;
        tournament.GameWonPoints = input.GameWonPoints;
        tournament.IsIndoor = input.IsIndoor;
        tournament.IsLeague = input.IsLeague;
        tournament.StartDate = input.StartDate;
        tournament.EndDate = input.EndDate;
        
        await SaveAsync(tournament);
        return true;
    }

    /// <summary>
    /// Change the avenue of a tournament
    /// </summary>
    /// <exception cref="MissingEntryException">When provided avenueId is missing from DB.</exception>
    public async Task<bool> ChangeAvenue(int tournamentId, int newAvenueId)
    {
        var avenue = await Data.Set<Avenue>().FindAsync(newAvenueId);
        if (avenue == null)
            throw new MissingEntryException($"Could not create tournament because avenue with id {newAvenueId} could not be found");
        
        var tournament = await Data.FindAsync<Tournament>(tournamentId);
        if (tournament == null)
            return false;

        tournament.Avenue = avenue;
        await SaveAsync(tournament);
        return true;
    }

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;
        
        var tournament = await Data.FindAsync<Tournament>(id);
        if (tournament == null)
            return false;

        await Delete(tournament, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;
        
        var tournament = await Data.FindAsync<Tournament>(id);
        if (tournament == null)
            return false;

        HardDelete(tournament);
        _logger.LogInformation("Tournament with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }

    private async Task<IEnumerable<Tournament>> QueryableToList(IQueryable<Tournament> queryable)
        => await queryable.Include(t => t.Avenue).Include(t => t.Organiser).ToListAsync();

    private IQueryable<Tournament> GetTournamentsQuery(TournamentQuery query, int? tournamentId = null)
    {
        var dataQuery = All().EnrichTournamentQueryData();

        if (tournamentId.HasValue)
        {
            dataQuery = dataQuery.Where( t=> t.Id == tournamentId);
            return dataQuery;
        }

        if (query.HasPrize ?? false)
            dataQuery = dataQuery.Where(t => t.Prize.HasValue);
        if (query.HasEntryFee ?? false)
            dataQuery = dataQuery.Where(t => t.EntryFee.HasValue);
        if (query.Surface.HasValue)
            dataQuery = dataQuery.Where(t => t.Surface == query.Surface);
        if (query.TournamentType.HasValue)
            dataQuery = dataQuery.Where(t => t.Type == query.TournamentType);
        if (query.IsIndoor.HasValue)
            dataQuery = dataQuery.Where(t => t.IsIndoor == query.IsIndoor);
        if (query.IsOngoingAtDateTime.HasValue)
            dataQuery = dataQuery.Where(t => t.StartDate <= query.IsOngoingAtDateTime && t.EndDate >= query.IsOngoingAtDateTime);
        if (query.DateRangeFrom.HasValue)
            dataQuery = dataQuery.Where(t => t.StartDate >= query.DateRangeFrom);
        if (query.DateRangeUntil.HasValue)
            dataQuery = dataQuery.Where(t => t.EndDate <= query.DateRangeUntil);
        if (query.OrganiserId.HasValue)
            dataQuery = dataQuery.Where(t => t.OrganiserId == query.OrganiserId.Value);
        if (query.ParticipantIds != null && query.ParticipantIds.Any())
            dataQuery = dataQuery.Where(t => t.Participants.Any(p => p.Players.Any(acc => query.ParticipantIds.Contains(acc.AccountId))));
        if (query.ParticipantUsernames != null && query.ParticipantUsernames.Any())
            dataQuery = dataQuery.Where(t => t.Participants.Any(p => p.Players.Any(acc => query.ParticipantUsernames.Contains(acc.Account.Username))));

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            dataQuery = dataQuery.Where(t => 
                t.Title.Contains(query.Keyword) ||
                t.Rules.Contains(query.Keyword) ||
                t.Description.Contains(query.Keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.City))
        {
            dataQuery = dataQuery.Where(t => t.Avenue.City.Contains(query.City));
        }

        dataQuery = dataQuery.SortQuery(query.SortOptions);

        return dataQuery;
    }
}