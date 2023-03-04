namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Exceptions;
using Extensions;
using Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models.Tournament;

public class TournamentsService : DeletableDataService<Tournament>, ITournamentsService
{
    private readonly IMapper _mapper;
    private readonly ILogger<TournamentsService> _logger;

    public TournamentsService(DbContext db, IMapper mapper, ILogger<TournamentsService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<IEnumerable<TournamentOutputModel>> GetAll()
        => await All().ProjectToType<TournamentOutputModel>().ToListAsync();

    public async Task<TournamentOutputModel> Get(int id)
        => await All().Where(a => a.Id == id).ProjectToType<TournamentOutputModel>().SingleOrDefaultAsync();

    public async Task<Tournament> GetInternal(int id)
        => await All().Where(a => a.Id == id).SingleOrDefaultAsync();

    public async Task<IEnumerable<TournamentOutputModel>> Query(TournamentQuery query)
        => (await GetAvenuesQuery(query)
                .ProjectToType<TournamentOutputModel>()
                .ToListAsync())
            .PageFilterResult(query);

    public async ValueTask<int> Total(TournamentQuery query) => await GetAvenuesQuery(query).CountAsync();

    /// <summary>
    /// Creates new tournament based on the input
    /// </summary>
    /// <exception cref="MissingEntryException">When provided avenueId is missing from DB.</exception>
    public async Task<int> Create(TournamentInputModel input, Account organiser)
    {
        var avenue = await Data.Set<Avenue>().FindAsync(input.AvenueId);
        if (avenue == null)
            throw new MissingEntryException($"Could not create tournament because avenue with id {input.AvenueId} could not be found");
        
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

    private IQueryable<Tournament> GetAvenuesQuery(TournamentQuery query, int? tournamentId = null)
    {
        var dataQuery = All();

        if (tournamentId.HasValue)
        {
            dataQuery = dataQuery.Where( t=> t.Id == tournamentId);
            return dataQuery;
        }

        if (query.HasPrize.HasValue)
            dataQuery = dataQuery.Where(t => t.Prize.HasValue);
        if (query.HasEntryFee.HasValue)
            dataQuery = dataQuery.Where(t => t.EntryFee.HasValue);
        if (query.Surface.HasValue)
            dataQuery = dataQuery.Where(t => t.Surface == query.Surface);
        if (query.TournamentType.HasValue)
            dataQuery = dataQuery.Where(t => t.Type == query.TournamentType);
        if (query.IsIndoor.HasValue)
            dataQuery = dataQuery.Where(t => t.IsIndoor == query.IsIndoor);
        if (query.DateRangeFrom.HasValue)
            dataQuery = dataQuery.Where(t => t.StartDate >= query.DateRangeFrom);
        if (query.DateRangeUntil.HasValue)
            dataQuery = dataQuery.Where(t => t.StartDate <= query.DateRangeUntil);

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            dataQuery = dataQuery.Where(t => 
                t.Title.ToLowerInvariant().Contains(query.Keyword) ||
                t.Rules.ToLowerInvariant().Contains(query.Keyword) ||
                t.Description.ToLowerInvariant().Contains(query.Keyword));
        }

        dataQuery = dataQuery.SortQuery(query.SortOptions);

        return dataQuery;
    }
}