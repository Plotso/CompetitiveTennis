namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data;
using Data.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class AccountsService : DataService<Account>, IAccountsService
{
    public AccountsService(DbContext db) : base(db)
    {
    }

    /// <summary>
    /// Retrieve PlayerRating for given account if there is such for current user
    /// </summary>
    /// <param name="userId">current user id</param>
    /// <exception cref="ArgumentException"></exception>
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
            throw new InvalidOperationException($"Could not take player rating for UserId: {userId}. ErrorMsg: {e.Message}");
        }
    }

    public async Task UpdatePlayerRating(string userId, int newRating)
    {
        try
        {
            var account = await All().SingleOrDefaultAsync(a => a.UserId == userId);
            if (account == null)
                throw new ArgumentException($"Cannot update rating for non-existing account. UserId: {userId}");

            account.PlayerRating = newRating;
            await SaveAsync(account);
        }
        catch (ArgumentException) { throw; }
        catch (Exception e)
        {
            throw new InvalidOperationException($"An error occured during {nameof(UpdatePlayerRating)} execution for userId: {userId}. Error: {e.Message} ");
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