namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Avenue;
using Services.Interfaces;

public class AvenuesController : ApiController
{
    private readonly IAvenuesService _avenues;
    private readonly ICurrentUserService _currentUser;

    public AvenuesController(IAvenuesService avenues, ICurrentUserService currentUser, ILogger<AvenuesController> logger) : base(logger)
    {
        _avenues = avenues;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvenueOutputModel>>> All()
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.GetAll();
                return Ok(avenues);
            },
            msgOnError: "An error occured during GET request for all avenues");

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvenueOutputModel>>> ById(int id)
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.Get(id);
                return Ok(avenues);
            },
            msgOnError: $"An error occured during GET request with for avenue: {id}");

    [HttpGet]
    public async Task<ActionResult<SearchAvenueOutputModel>> Search([FromQuery] AvenueQuery query)
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.Query(query);
                var total = await _avenues.Total(query);
                return Ok(new SearchAvenueOutputModel(avenues, query.Page, total));
            },
            msgOnError: $"An error occured during Search request with query: {query}");

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Add(AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                await _avenues.Create(input);
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during avenue creation. AvenueInput: {input}");

    [HttpPut]
    [Authorize(Roles = Constants.AdministratorRoleName)]
    [Route(Id)]
    public async Task<ActionResult> Edit(int id, AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                if (!_currentUser.IsAdministrator)
                    return BadRequest(Result.Failure("Only admins are allowed to update avenues!"));

                var isSuccess = await _avenues.Update(id, input);
                return isSuccess ? Result.Success : Result.Failure($"Update for avenue {id} failed.");
            },
            msgOnError: $"Unexpected error during avenue update. AvenueInput: {input}. AvenueID: {id}",
            msgOnNotFound: NotFoundMsg(id, "update"));

    [HttpDelete]
    [Authorize(Roles = Constants.AdministratorRoleName)]
    [Route(Id)]
    public async Task<ActionResult> Delete(int id)
        => await SafeHandle(async () =>
            {
                if (!_currentUser.IsAdministrator)
                    return BadRequest(Result.Failure("Only admins are allowed to delete avenues!"));

                var isSuccess = await _avenues.Delete(id, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Delete operation for avenue {id} failed.");
            },
            msgOnError: $"Unexpected error during avenue delete. AvenueID: {id}",
            msgOnNotFound: NotFoundMsg(id, "delete"));

    private string NotFoundMsg(int id, string operationName) => $"Entry not found error occurred during {operationName} operation for Avenue with ID: {id}";
}