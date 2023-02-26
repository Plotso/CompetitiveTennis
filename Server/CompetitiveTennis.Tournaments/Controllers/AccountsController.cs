namespace CompetitiveTennis.Tournaments.Controllers;

using System.Net;
using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Account;
using Services.Interfaces;
using static ServiceConstants;

public class AccountsController : ApiController
{
    private readonly IAccountsService _accounts;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountsService accounts, ICurrentUserService currentUser, ILogger<AccountsController> logger)
    {
        _accounts = accounts;
        _currentUser = currentUser;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add(AccountInputModel input)
    {
        try
        {
            await _accounts.Create(new AccountCreateInputModel(_currentUser.UserId, _currentUser.Username, input));
            return Ok(Result.Success);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during public post creation. UserID: {userId}", _currentUser.UserId);
            return StatusCode((int)HttpStatusCode.InternalServerError ,Result.Failure($"ErrorCode: {UnexpectedErrorCode}"));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangeNames(AccountInputModel input)
    {
        try
        {
            var account = await _accounts.GetByUserId(_currentUser.UserId);
            if (account == null)
                return NotFound(Result.Failure("User {_currentUser.Username} has no account inside the system"));
            account.FirstName = input.FirstName;
            account.LastName = input.LastName;
            await _accounts.SaveAsync(account);
            return Ok(Result.Success);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during public post creation. UserID: {userId}", _currentUser.UserId);
            return StatusCode((int)HttpStatusCode.InternalServerError ,Result.Failure($"ErrorCode: {UnexpectedErrorCode}"));
        }
    }
}