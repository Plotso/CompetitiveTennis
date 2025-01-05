namespace CompetitiveTennis.Tournaments.Services.Data;

using System.Text.Json;
using CompetitiveTennis.Data;
using Exceptions;
using Contracts.Avenue;
using CompetitiveTennis.Tournaments.Data.Models;
using Extensions;
using SerializerOptions;
using Interfaces.Data;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class AvenuesService : DeletableDataService<Avenue>, IAvenuesService
{
    //ToDo: Add option for administrators to verify and mark as active/inactive avenues
    private readonly IMapper _mapper;
    private readonly ILogger<AvenuesService> _logger;
    private readonly ISerializerOptions _serializerOptions;

    public AvenuesService(DbContext db, IMapper mapper, ILogger<AvenuesService> logger, ISerializerOptions serializerOptions) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
        _serializerOptions = serializerOptions;
    }

    public async Task<IEnumerable<AvenueOutputModel>> GetAll()
        => await All()
            .ProjectToType<AvenueOutputModel>()
            .ToListAsync();

    public async Task<IEnumerable<AvenueOutputModel>> Query(AvenueQuery query)
        => (query.Surface == null && query.CourtType == null ?
            await GetAvenuesQuery(query)
                .ProjectToType<AvenueOutputModel>()
                .ToListAsync() :
            FilterAvenuesByCourtInfo(GetAvenuesQuery(query).Include(a => a.Tournaments), query)
            ).PageFilterResult(query) ;

    public async ValueTask<int> Total(AvenueQuery query) 
        => query.Surface == null && query.CourtType == null ?
            await GetAvenuesQuery(query).CountAsync() :
            FilterAvenuesByCourtInfo(GetAvenuesQuery(query), query).Count();

    public async Task<AvenueOutputModel> Get(int id) 
        => await All()
            .Where(a => a.Id == id)
            .Include(a => a.Tournaments)
            .ProjectToType<AvenueOutputModel>()
            .SingleOrDefaultAsync();

    public async Task<Avenue> GetInternal(int id)
        => await All().Where(a => a.Id == id).SingleOrDefaultAsync();

    public async Task<int> Create(AvenueInputModel input, string userId)
    {
        var unifiedCourtsInfo = input.Courts.UnifyCourtsInfo();
        var avenue = _mapper.Map<Avenue>(input);
        avenue.IsActive = true;
        avenue.IsVerified = false;
        avenue.CreatedBy = userId;
        avenue.Courts = JsonSerializer.Serialize(unifiedCourtsInfo, _serializerOptions.GetSerializerOptions());

        await SaveAsync(avenue);
        return avenue.Id;
    }

    public async Task<bool> ValidateAvenueInfoCanBeApplied(AvenueInputModel input, int? id = null)
    {
        var idToSearch = id ?? -1;
        //ToDo: Logic for checking whether an avenue with given name and/or location exists that is different than our current entity. The reason for that is we have unique constraints in the table so DB will throw error. It can be caught on clientSide instead
        throw new NotImplementedException();
    }

    public async Task<bool> Update(int id, AvenueInputModel input, string userId)
    {
        var avenue = await Data.FindAsync<Avenue>(id);
        if (avenue == null)
            throw new MissingEntryException($"No avenue found with Id: {id}");

        // good place to add validation for the unique fields name & location
        avenue.Name = input.Name;
        avenue.Location = input.Location;
        avenue.City = input.City;
        avenue.Country = input.Country;
        avenue.Details = input.Details;
        avenue.UpdatedBy = userId;
        var unifiedCourtsInfo = input.Courts.UnifyCourtsInfo();
        avenue.Courts = JsonSerializer.Serialize(unifiedCourtsInfo, _serializerOptions.GetSerializerOptions());

        await SaveAsync(avenue);
        return true;
    }

    public async Task<bool> Delete(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;
        
        var avenue = await Data.FindAsync<Avenue>(id);
        if (avenue == null)
            return false;

        await Delete(avenue, userid);
        return true;
    }

    public async Task<bool> DeletePermanently(int id, string userid)
    {
        if (string.IsNullOrWhiteSpace(userid))
            return false;
        
        var avenue = await Data.FindAsync<Avenue>(id);
        if (avenue == null)
            return false;

        HardDelete(avenue);
        _logger.LogInformation("Avenue with id: {Id} has been permanently deleted by UserId: {Userid}", id, userid);
        await Data.SaveChangesAsync();
        return true;
    }

    private IEnumerable<AvenueOutputModel> FilterAvenuesByCourtInfo(IQueryable<Avenue> dbAvenues, AvenueQuery query)
    {
        var avenues = dbAvenues.ProjectToType<AvenueOutputModel>().ToList();

        if (query.Surface.HasValue)
        {
            avenues = avenues.Where(a => a.Courts.Any(c => c.Surface == query.Surface)).ToList();
        }

        if (query.CourtType.HasValue)
        {
            avenues = avenues
                .Where(a => a.Courts.Any(c =>
                        c.AvailableCourtsByType.ContainsKey(query.CourtType.Value) &&
                        c.AvailableCourtsByType[query.CourtType.Value] > 0)).ToList();
        }

        return avenues;
    }

    private IQueryable<Avenue> GetAvenuesQuery(AvenueQuery query, int? avenueId = null)
    {
        var dataQuery = All();

        if (avenueId.HasValue)
        {
            dataQuery = dataQuery.Where(a => a.Id == avenueId);
            return dataQuery;
        }

        if (!string.IsNullOrWhiteSpace(query.City))
        {
            dataQuery = dataQuery.Where(a => a.City == query.City);
        }

        if (!string.IsNullOrWhiteSpace(query.Country))
        {
            dataQuery = dataQuery.Where(a => a.Country == query.Country);
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            dataQuery = dataQuery.Where(a => 
                a.Name.Contains(query.Keyword) ||
                a.Location.Contains(query.Keyword) ||
                a.City.Contains(query.Keyword) ||
                a.Country.Contains(query.Keyword));
        }

        dataQuery = dataQuery.SortQuery(query.SortOptions);

        return dataQuery;
    }
}