namespace CompetitiveTennis.Tournaments.Controllers;

using System.Net;
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

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add(AccountInputModel input)
        => await SafeHandle(async () =>
            {
                await _accounts.Create(new AccountCreateInputModel(_currentUser.UserId, _currentUser.Username, input));
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during internal account creation. UserID: {_currentUser.UserId}");

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeNames(AccountInputModel input)
        => await SafeHandle(async () =>
            {
                var account = await _accounts.GetByUserId(_currentUser.UserId);
                if (account == null)
                    return NotFound(Result.Failure($"User {_currentUser.Username} has no account inside the system"));
                account.FirstName = input.FirstName;
                account.LastName = input.LastName;
                await _accounts.SaveAsync(account);
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during internal account name update. UserID: {_currentUser.UserId}");
}