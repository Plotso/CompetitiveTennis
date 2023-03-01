namespace CompetitiveTennis.Tournaments.Services;

using System.Text.Json;
using CompetitiveTennis.Data;
using CompetitiveTennis.Models;
using Data.Models;
using Exceptions;
using Extensions;
using Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models.Avenue;

public class AvenuesService : DeletableDataService<Avenue>, IAvenuesService
{
    //ToDo: Add option for administrators to verify and mark as active/inactive avenues
    private readonly IMapper _mapper;
    private readonly ILogger<AvenuesService> _logger;

    public AvenuesService(DbContext db, IMapper mapper, ILogger<AvenuesService> logger) : base(db)
    {
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AvenueOutputModel>> GetAll()
        => await All().ProjectToType<AvenueOutputModel>().ToListAsync();

    public async Task<IEnumerable<AvenueOutputModel>> Query(AvenueQuery query)
        => (query.Surface == null && query.CourtType == null ?
            await GetAvenuesQuery(query).ProjectToType<AvenueOutputModel>().ToListAsync() :
            FilterAvenuesByCourtInfo(GetAvenuesQuery(query), query)
            ).PageFilterResult(query) ;

    public async ValueTask<int> Total(AvenueQuery query) 
        => query.Surface == null && query.CourtType == null ?
            await GetAvenuesQuery(query).CountAsync() :
            FilterAvenuesByCourtInfo(GetAvenuesQuery(query), query).Count();

    public async Task<AvenueOutputModel> Get(int id) 
        => await All().Where(a => a.Id == id).ProjectToType<AvenueOutputModel>().SingleOrDefaultAsync();

    public async Task<int> Create(AvenueInputModel input)
    {
        var unifiedCourtsInfo = input.Courts.UnifyCourtsInfo();
        // var avenue = new Avenue
        // {
        //     Name = input.Name,
        //     Location = input.Location,
        //     City = input.City,
        //     Country = input.Country,
        //     Details = input.Details
        // }
        var avenue = _mapper.Map<Avenue>(input);
        avenue.IsActive = true;
        avenue.IsVerified = false;
        avenue.Courts = JsonSerializer.Serialize(unifiedCourtsInfo, SerializerOptions.StringEnumOptions());

        await SaveAsync(avenue);
        return avenue.Id;
    }

    public async Task<bool> ValidateAvenueInfoCanBeApplied(AvenueInputModel input, int? id = null)
    {
        var idToSearch = id ?? -1;
        //ToDo: Logic for checking whether an avenue with given name and/or location exists that is different than our current entity. The reason for that is we have unique constraints in the table so DB will throw error. It can be caught on clientSide instead
        throw new NotImplementedException();
    }

    public async Task<bool> Update(int id, AvenueInputModel input)
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
        var unifiedCourtsInfo = input.Courts.UnifyCourtsInfo();
        avenue.Courts = JsonSerializer.Serialize(unifiedCourtsInfo, SerializerOptions.StringEnumOptions());

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
                a.Name.ToLowerInvariant().Contains(query.Keyword) ||
                a.Location.ToLowerInvariant().Contains(query.Keyword) ||
                a.City.ToLowerInvariant().Contains(query.Keyword) ||
                a.Country.ToLowerInvariant().Contains(query.Keyword));
        }

        dataQuery = SortQuery(dataQuery, query.SortOptions);

        return dataQuery;
    }

    private IQueryable<Avenue> SortQuery(IQueryable<Avenue> query, SortOptions options) => options switch
    {
        SortOptions.CreatedDescending => query.OrderByDescending( a=> a.CreatedOn),
        SortOptions.UpdatedAscending => query.OrderBy(a => a.ModifiedOn),
        SortOptions.UpdatedDescending => query.OrderByDescending(a => a.ModifiedOn),
        _ => query // CreatedAscending, default ordering
    };
}