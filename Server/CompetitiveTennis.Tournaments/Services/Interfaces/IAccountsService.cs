namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using System.Collections;
using CompetitiveTennis.Data;
using Data.Models;
using Exceptions;
using Models;
using Models.Account;

public interface IAccountsService : IDataService<Account>
{
    Task<IEnumerable<AccountOutputModel>> GetAll();
    Task<AccountOutputModel> GetById(int id);
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
    Task UpdatePlayerRating(string userId, int newRating);
    Task Create(AccountCreateInputModel createModel);
}