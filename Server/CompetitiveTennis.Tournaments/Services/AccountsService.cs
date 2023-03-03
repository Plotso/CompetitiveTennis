namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Exceptions;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Account;
using static ServiceConstants;

public class AccountsService : DataService<Account>, IAccountsService
{
    public AccountsService(DbContext db) : base(db)
    {
    }

    /// <summary>
    /// Retrieve PlayerRating for given account if there is such for current user
    /// </summary>
    /// <param name="userId">current user id</param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<int> GetPlayerRating(string userId)
    {
        try
        {
            return await All()
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
    public async Task UpdatePlayerRating(string userId, int newRating)
    {
        try
        {
            var account = await All().SingleOrDefaultAsync(a => a.UserId == userId);
            if (account == null)
                throw new MissingEntryException($"Cannot update rating for non-existing account. UserId: {userId}");

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
                $"An error occured during {nameof(UpdatePlayerRating)} execution for userId: {userId}. Error: {e.Message} ");
        }
    }

    public async Task Create(AccountCreateInputModel createModel)
    {
        var existingAccount = await GetByUserId(createModel.UserId);
        if (existingAccount == null)
        {
            var account = new Account
            {
                FirstName = createModel.Input.FirstName,
                LastName = createModel.Input.LastName,
                UserId = createModel.UserId,
                Username = createModel.Username,
                PlayerRating = DefaultPlayerRating
            };
            await SaveAsync(account);
        }
    }

    public async Task<Account?> GetByUserId(string userId)
        => await All()
            .Where(a => a.UserId == userId)
            .SingleOrDefaultAsync();

    public async Task<Account?> GetSystemUser()
        => await All()
            .Where(a => a.Username == Constants.SystemUser)
            .SingleOrDefaultAsync();
}