namespace CompetitiveTennis.Tournaments.Services.Interfaces.Data;

using CompetitiveTennis.Data;
using Exceptions;
using Contracts.Account;
using CompetitiveTennis.Tournaments.Data.Models;

public interface IAccountsService : IDataService<Account>
{
    Task<IEnumerable<AccountOutputModel>> GetAll();
    Task<AccountOutputModel> GetById(int id);
    Task<AccountOutputModel> GetByUsernamme(string username);
    Task<IEnumerable<Account>> GetMultiple(IEnumerable<int> ids);
    Task<Account?> GetByUserId(string userId);
    Task<Account?> GetInternal(int id);
    Task<Account?> GetSystemUser();
    Task<int> GetPlayerRating(string userId);
    /// <summary>
    /// Updates the player rating for given user
    /// </summary>
    /// <exception cref="MissingEntryException">In case provided user cannot be located in the DB</exception>
    /// <exception cref="InvalidOperationException">Internal service or infrastructure error</exception>
    Task UpdatePlayerRating(string userId, int newRating, bool isDoubles);
    /// <exception cref="MissingEntryException">In case provided user cannot be located in the DB</exception>
    /// <exception cref="InvalidOperationException">Internal service or infrastructure error</exception>
    Task UpdatePlayerRating(int accountId, int newRating, bool isDoubles);
    Task Create(AccountCreateInputModel createModel);
}