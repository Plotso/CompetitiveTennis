namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Contracts.Account;
using Refit;

public interface IAccountsService
{
    [Get("/Accounts/All")]
    Task<Result<IEnumerable<AccountOutputModel>>> All();
    
    [Get("/Accounts/{id}")]
    Task<Result<AccountOutputModel>> ById(int id);
    
    [Get("/Accounts/{username}")]
    Task<Result<AccountOutputModel>> ByUsername(string username);

    [Post("/Accounts/Add")]
    Task<Result> Add(AccountInputModel input);
    
    [Put("/Accounts/ChangeNames/{id}")]
    Task<Result> ChangeNames(int id, AccountInputModel input);
    
}