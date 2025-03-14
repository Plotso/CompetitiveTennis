﻿namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contracts.Avenue;
using Services.Interfaces.Data;
using Constants = Constants;

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
    public async Task<ActionResult<Result<IEnumerable<AvenueOutputModel>>>> All()
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.GetAll();
                return Ok(Result<IEnumerable<AvenueOutputModel>>.SuccessWith(avenues));
            },
            msgOnError: "An error occurred during GET request for all avenues");

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<AvenueOutputModel>> ById(int id)
        => await SafeHandle(async () =>
            {
                var avenue = await _avenues.Get(id);
                if (avenue == null)
                    return NotFound(Result.Failure($"Avenue {id} is missing"));
                return Ok(Result<AvenueOutputModel>.SuccessWith(avenue));
            },
            msgOnError: $"An error occurred during GET request for avenue: {id}");

    [HttpGet]
    [Route(nameof(Search))]
    public async Task<ActionResult<Result<SearchOutputModel<AvenueOutputModel>>>> Search([FromQuery] AvenueQuery query)
        => await SafeHandle(async () =>
            {
                var avenues = await _avenues.Query(query);
                var total = await _avenues.Total(query);
                return Ok(Result<SearchOutputModel<AvenueOutputModel>>.SuccessWith(
                    new SearchOutputModel<AvenueOutputModel>(avenues, query.Page, total)));
            },
            msgOnError: $"An error occurred during Search request with query: {query}");

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Result<int>>> Add(AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                var avenueId = await _avenues.Create(input, _currentUser.UserId);
                return Ok(Result<int>.SuccessWith(avenueId));
            },
            msgOnError: $"Unexpected error during avenue creation. AvenueInput: {input}");

    [HttpPut]
    [Route(Id)]
    [Authorize(Roles = Constants.AdministratorRoleName)]
    public async Task<ActionResult> Edit(int id, AvenueInputModel input)
        => await SafeHandle(async () =>
            {
                if (!_currentUser.IsAdministrator)
                    return Unauthorized(Result.Failure("Only admins are allowed to update avenues!"));

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
                    return Unauthorized(Result.Failure("Only admins are allowed to delete avenues!"));

                var isSuccess = await _avenues.Delete(id, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Delete operation for avenue {id} failed.");
            },
            msgOnError: $"Unexpected error during avenue delete. AvenueID: {id}",
            msgOnNotFound: NotFoundMsg(id, "delete"));

    private string NotFoundMsg(int id, string operationName) => $"Entry not found error occurred during {operationName} operation for Avenue with ID: {id}";
}