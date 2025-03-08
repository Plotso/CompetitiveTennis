namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Contracts;
using Contracts.Account;
using Refit;

public interface IAccountsService
{
    [Get("/Accounts/Search")]
    Task<Result<SearchOutputModel<AccountOutputModel>>> Search([Query] AccountQuery query);
    
    [Get("/Accounts/All")]
    Task<Result<IEnumerable<AccountOutputModel>>> All();
    
    [Get("/Accounts/{id}")]
    Task<Result<AccountOutputModel>> ById(int id);
    
    [Get("/Accounts/{username}")]
    Task<Result<AccountOutputModel>> ByUsername(string username);

    [Get("/Accounts/Stats/{username}")]
    Task<Result<AccountStats>> Stats(string username);

    [Post("/Accounts/Add")]
    Task<Result> Add(AccountInputModel input);
    
    [Put("/Accounts/ChangeNames/{id}")]
    Task<Result> ChangeNames(int id, AccountInputModel input);
    
}