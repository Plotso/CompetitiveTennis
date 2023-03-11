namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using Services.Interfaces;

public class AccountsController : ApiController
{
    private readonly IAccountsService _accounts;
    private readonly ICurrentUserService _currentUser;

    public AccountsController(IAccountsService accounts, ICurrentUserService currentUser, ILogger<AccountsController> logger) : base(logger)
    {
        _accounts = accounts;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Route(nameof(All))]
    public async Task<ActionResult<IEnumerable<AccountOutputModel>>> All()
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetAll();
                return Ok(account);
            },
            msgOnError: "An error occured during GET request for all accounts");
    
    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<AccountOutputModel>> ById(int id) 
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetById(id);
                if (account == null)
                    return NotFound($"Account {id} is missing");
                return Ok(account);
            },
            msgOnError: $"An error occured during GET request for account: {id}");

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add(AccountInputModel input)
        => await SafeHandle(async () =>
            {
                await _accounts.Create(new AccountCreateInputModel(_currentUser.UserId, _currentUser.Username, input));
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during internal account creation. UserID: {_currentUser.UserId}");

    [HttpPut]
    [Route($"{nameof(ChangeNames)}/{Id}")]
    [Authorize]
    public async Task<ActionResult> ChangeNames(int id, AccountInputModel input)
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetByUserId(_currentUser.UserId);
                if (account == null)
                    return NotFound(Result.Failure($"User {_currentUser.Username} has no account inside the system"));
                if (!_currentUser.IsAdministrator && account.Id != id)
                    return BadRequest(Result.Failure("Current user doesn't have rights to change names for provided account."));
                
                account.FirstName = input.FirstName;
                account.LastName = input.LastName;
                await _accounts.SaveAsync(account);
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during internal account name update. UserID: {_currentUser.UserId}");
}