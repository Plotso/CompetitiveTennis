﻿namespace CompetitiveTennis.Tournaments.Services.Data;

using CompetitiveTennis.Data;
using CompetitiveTennis.Data.Models.Enums;
using Exceptions;
using Contracts.Account;
using CompetitiveTennis.Tournaments.Data.Models;
using Extensions;
using Interfaces.Data;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using Tournaments.Data;
using static ServiceConstants;

public class AccountsService : DataService<Account>, IAccountsService
{
    private readonly IMapper _mapper;

    public AccountsService(IMapper mapper, DbContext db) : base(db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountOutputModel>> Query(AccountQuery query)
        => _mapper.Map<IEnumerable<AccountOutputModel>>(await GetAccountsQuery(query).PageFilterResult(query)
            .ToListAsync());

    public async ValueTask<int> Total(AccountQuery query) => await GetAccountsQuery(query).CountAsync();

    public async Task<IEnumerable<AccountOutputModel>> GetAll()
        => await AllAsNoTracking()
            .ProjectToType<AccountOutputModel>()
            .ToListAsync();

    public async Task<AccountOutputModel> GetById(int id)
        => await AllAsNoTracking()
            .Where(a => a.Id == id)
            .Include(a => a.Participations)
            .ThenInclude(p => p.Participant)
            .Include(a => a.OrganisedTournaments)
            .ProjectToType<AccountOutputModel>()
            .SingleOrDefaultAsync();

    public async Task<AccountOutputModel> GetByUsername(string username)
        => await AllAsNoTracking()
            .Where(a => a.Username == username)
            .Include(a => a.Participations)
            .ThenInclude(p => p.Participant)
            .Include(a => a.OrganisedTournaments)
            .ProjectToType<AccountOutputModel>()
            .SingleOrDefaultAsync();

    public async Task<AccountRatingInfo> GetRatingForUsername(string username)
        => await AllAsNoTracking()
        .Where(a => a.Username == username)
        .ProjectToType<AccountRatingInfo>()
        .FirstOrDefaultAsync();

    public async Task<bool> HasAccountWithUsername(string username)
        => await AllAsNoTracking()
            .AnyAsync(a => a.Username == username);

    /// <summary>
    /// Retrieve PlayerRating for given account if there is such for current user
    /// </summary>
    /// <param name="userId">current user id</param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<int> GetPlayerRating(string userId)
    {
        try
        {
            return await AllAsNoTracking()
                .Where(a => a.UserId == userId)
                .Select(a => a.PlayerRating)
                .SingleOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Could not take player rating for UserId: {userId}. ErrorMsg: {e.Message}");
        }
    }

    /// <summary>
    /// Updates the player rating for given user
    /// </summary>
    /// <exception cref="MissingEntryException">In case provided user cannot be located in the DB</exception>
    /// <exception cref="InvalidOperationException">Internal service or infrastructure error</exception>
    public async Task UpdatePlayerRating(string userId, int newRating, bool isDoubles)
    {
        try
        {
            var account = await All().SingleOrDefaultAsync(a => a.UserId == userId);
            if (account == null)
                throw new MissingEntryException($"Cannot update rating for non-existing account. UserId: {userId}");

            if(isDoubles)
                account.DoublesRating = newRating;
            else
                account.PlayerRating = newRating;
            await SaveAsync(account);
        }
        catch (MissingEntryException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"An error occurred during {nameof(UpdatePlayerRating)} execution for userId: {userId}. Error: {e.Message} ");
        }
    }

    public async Task UpdatePlayerRating(int accountId, int newRating, bool isDoubles)
    {
        try
        {
            var account = await All().SingleOrDefaultAsync(a => a.Id == accountId);
            if (account == null)
                throw new MissingEntryException($"Cannot update rating for non-existing account. AccountId: {accountId}");

            if(isDoubles)
                account.DoublesRating = newRating;
            else
                account.PlayerRating = newRating;
            await SaveAsync(account);
        }
        catch (MissingEntryException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"An error occurred during {nameof(UpdatePlayerRating)} execution for accountId: {accountId}. Error: {e.Message} ");
        }
    }

    public async Task Create(AccountCreateInputModel createModel)
    {
        var existingAccount = await AllAsNoTracking().AnyAsync(a => a.UserId == createModel.UserId);
        if (!existingAccount)
        {
            var account = new Account
            {
                FirstName = createModel.Input.FirstName,
                LastName = createModel.Input.LastName,
                UserId = createModel.UserId,
                Username = createModel.Username,
                PlayerRating = DefaultPlayerRating,
                DoublesRating = DefaultPlayerRating
            };
            await SaveAsync(account);
        }
    }

    public async Task<IEnumerable<Account>> GetMultiple(IEnumerable<int> ids)
        => await All().Where(a => ids.Contains(a.Id)).ToListAsync();

    public async Task<Account?> GetByUserId(string userId)
        => await All()
            .Where(a => a.UserId == userId)
            .SingleOrDefaultAsync();

    public async Task<Account?> GetInternal(int id)
        => await All()
            .Where(a => a.Id == id)
            .SingleOrDefaultAsync();

    public async Task<Account?> GetSystemUser()
        => await All()
            .Where(a => a.Username == Constants.SystemUser)
            .SingleOrDefaultAsync();
    
    private IQueryable<Account> GetAccountsQuery(AccountQuery query, int? accountId = null)
    {
        var dataQuery = All();

        if (accountId.HasValue)
        {
            dataQuery = dataQuery.Where( t=> t.Id == accountId);
            return dataQuery;
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            dataQuery = dataQuery.Where(a => 
                a.FirstName.Contains(query.Keyword) ||
                a.LastName.Contains(query.Keyword) ||
                a.Username.Contains(query.Keyword));
        }

        // Sort query
        dataQuery = query.AdditionalSortOptions is not null ?
            dataQuery.SortAccounts(query.AdditionalSortOptions.Value) :
            dataQuery.SortQuery(query.SortOptions);

        return dataQuery;
    }
}