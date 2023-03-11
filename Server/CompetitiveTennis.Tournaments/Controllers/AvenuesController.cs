namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
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
    [Route(nameof(All))]
    public async Task<ActionResult<IEnumerable<AvenueOutputModel>>> All()
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.GetAll();
                return Ok(avenues);
            },
            msgOnError: "An error occured during GET request for all avenues");

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<AvenueOutputModel>> ById(int id)
        => await SafeHandle(async () =>
            {
                var avenue = await _avenues.Get(id);
                if (avenue == null)
                    return NotFound($"Avenue {id} is missing");
                return Ok(avenue);
            },
            msgOnError: $"An error occured during GET request for avenue: {id}");

    [HttpGet]
    [Route(nameof(Search))]
    public async Task<ActionResult<SearchOutputModel<AvenueOutputModel>>> Search([FromQuery] AvenueQuery query)
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.Query(query);
                var total = await _avenues.Total(query);
                return Ok(new SearchOutputModel<AvenueOutputModel>(avenues, query.Page, total));
            },
            msgOnError: $"An error occured during Search request with query: {query}");

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<int>> Add(AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                await _avenues.Create(input, _currentUser.UserId);
                return Ok(Result.Success);
            },
            msgOnError: $"Unexpected error during avenue creation. AvenueInput: {input}");

    [HttpPut]
    [Route(Id)]
    [Authorize(Roles = Constants.AdministratorRoleName)]
    public async Task<ActionResult> Edit(int id, AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                if (!_currentUser.IsAdministrator)
                    return BadRequest(Result.Failure("Only admins are allowed to update avenues!"));

                var isSuccess = await _avenues.Update(id, input, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Update for avenue {id} failed.");
            },
            msgOnError: $"Unexpected error during avenue update. AvenueInput: {input}. AvenueID: {id}",
            msgOnNotFound: NotFoundMsg(id, "update"));

    [HttpDelete]
    [Route(Id)]
    [Authorize(Roles = Constants.AdministratorRoleName)]
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