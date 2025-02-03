namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contracts.Account;
using Services.Interfaces.BL;
using Services.Interfaces.Data;

public class AccountsController : ApiController
{
    private readonly IAccountsService _accounts;
    private readonly ICurrentUserService _currentUser;
    private readonly IAccountStatsProvider _accountStatsProvider;

    public AccountsController(IAccountsService accounts, ICurrentUserService currentUser, IAccountStatsProvider accountStatsProvider, ILogger<AccountsController> logger) : base(logger)
    {
        _accounts = accounts;
        _currentUser = currentUser;
        _accountStatsProvider = accountStatsProvider;
    }

    [HttpGet]
    [Route(nameof(All))]
    public async Task<ActionResult<Result<IEnumerable<AccountOutputModel>>>> All()
        => await SafeHandle(async () =>
            {
                var accounts = await _accounts.GetAll();
                return Ok(Result<IEnumerable<AccountOutputModel>>.SuccessWith(accounts));
            },
            msgOnError: "An error occurred during GET request for all accounts");
    
    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<Result<AccountOutputModel>>> ById(int id) 
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetById(id);
                if (account == null)
                    return NotFound(Result.Failure($"Account {id} is missing"));
                return Ok(Result<AccountOutputModel>.SuccessWith(account));
            },
            msgOnError: $"An error occurred during GET request for account: {id}");
    
    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult<Result<AccountOutputModel>>> ByUsername(string username) 
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetByUsername(username);
                if (account == null)
                    return NotFound(Result.Failure($"Account {username} is missing"));
                return Ok(Result<AccountOutputModel>.SuccessWith(account));
            },
            msgOnError: $"An error occurred during GET request for account: {username}");
    
    [HttpGet]
    [Route($"{nameof(Stats)}/{Username}")]
    public async Task<ActionResult<Result<AccountStats>>> Stats(string username) 
        => await SafeHandle(async () =>
            {
                if (string.IsNullOrWhiteSpace(username))
                    return BadRequest(Result.Failure($"Username is required"));
                var hasAccountWithUsername = await _accounts.HasAccountWithUsername(username);
                if (!hasAccountWithUsername)
                    return NotFound(Result.Failure($"Account {username} is missing"));
                var account = await _accountStatsProvider.GetAccountStats(username);
                if (account == null)
                    return Result.Failure($"Failed to get account stats for {username}");
                return Ok(Result<AccountStats>.SuccessWith(account));
            },
            msgOnError: $"An error occurred during GET request for account: {username}");

    [HttpPost]
    [Authorize]
    [Route(nameof(Add))]
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