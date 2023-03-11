namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using Microsoft.AspNetCore.Mvc;
using Models.Account;
using Refit;

public interface IAccountsService
{
    [Get("/Accounts/All")]
    Task<ActionResult<IEnumerable<AccountOutputModel>>> All();
    
    [Get("/Accounts/{id}")]
    Task<ActionResult<IEnumerable<AccountOutputModel>>> ById(int id);

    [Post("/Accounts")]
    Task<ActionResult> Add(AccountInputModel input);
    
    [Post("/Accounts/ChangeNames/{id}")]
    Task<ActionResult> ChangeNames(int id, AccountInputModel input);
    
}