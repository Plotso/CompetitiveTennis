namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using CompetitiveTennis.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Avenue;
using Services.Tournaments;

public class AvenuesController : BaseGatewayController
{
    private readonly IAvenuesService _avenues;

    public AvenuesController(IAvenuesService avenues, ILogger logger) 
        : base(logger) 
        => _avenues = avenues;

    [HttpGet]
    [Route(nameof(All))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AvenueOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> All()
        => await SafeProcessRefitRequest(
            async () =>
            {
                var avenues = await _avenues.All();
                return Ok(avenues);
            }, "An error occurred while trying to get all avenues");
    
    [HttpGet]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AvenueOutputModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ById(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var avenue = await _avenues.ById(id);
                return Ok(avenue);
            }, $"An error occured during GET request for avenue: {id}");
    
    [HttpGet]
    [Route(nameof(Search))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchOutputModel<AvenueOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] AvenueQuery query)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var avenues = await _avenues.Search(query);
                return Ok(avenues);
            }, $"An error occured during avenues search with query: {query}");
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add(AvenueInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var avenueId = await _avenues.Create(input);
                return Ok(avenueId);
            }, $"An error occured during Create request for avenue. Request: {input}");
    
    [HttpPut]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(int id, AvenueInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _avenues.Edit(id, input);
                return Ok(result);
            }, $"An error occured during Edit request for avenue: {id}. Request: {input}");
    
    [HttpDelete]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _avenues.Delete(id);
                return Ok(result);
            }, $"An error occured during Delete request for avenue: {id}");
}