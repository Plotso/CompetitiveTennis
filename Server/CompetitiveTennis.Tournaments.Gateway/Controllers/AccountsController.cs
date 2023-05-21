namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using CompetitiveTennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Account;
using Services.Tournaments;

public class AccountsController : BaseGatewayController
{
    private readonly IAccountsService _accounts;

    public AccountsController(IAccountsService accounts, ILogger<AccountsController> logger) 
        : base(logger) 
        => _accounts = accounts;

    [HttpGet]
    [Route(nameof(All))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> All()
        => await SafeProcessRefitRequest(
            async () =>
            {
                var accounts = await _accounts.All();
                return Ok(accounts);
            }, "An error occurred while trying to get all accounts");
    
    [HttpGet]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountOutputModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ById(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var account = await _accounts.ById(id);
                return Ok(account);
            }, $"An error occured during GET request for account: {id}");
    
    [HttpGet]
    [Route("{username}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountOutputModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ByUsername(string username) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var account = await _accounts.ByUsername(username);
                return Ok(account);
            }, $"An error occured during GET request for account: {username}");

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add(AccountInputModel input)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _accounts.Add(input);
                return new ObjectResult(result);
            }, $"An error occurred while trying to create account with data {input}");

    [HttpPut]
    [Route($"{nameof(ChangeNames)}/{Id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeNames(int id, AccountInputModel input)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _accounts.ChangeNames(id, input);
                return new ObjectResult(result);
            }, $"An error occurred while trying to ChangeNames for account with data {input}");
}