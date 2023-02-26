namespace CompetitiveTennis.Tournaments.Services.Interfaces;

using CompetitiveTennis.Data;
using Data.Models;
using Models;
using Models.Account;

public interface IAccountsService : IDataService<Account>
{
    Task<Account?> GetByUserId(string userId);
    Task<Account?> GetSystemUser();
    Task<int> GetPlayerRating(string userId);
    Task UpdatePlayerRating(string userId, int newRating);
    Task Create(AccountCreateInputModel createModel);
}