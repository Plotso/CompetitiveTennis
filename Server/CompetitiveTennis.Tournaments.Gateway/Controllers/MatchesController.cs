namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using CompetitiveTennis.Models;
using Contracts;
using Contracts.Match;
using Contracts.MatchPeriod;
using Contracts.Tournament;
using Microsoft.AspNetCore.Mvc;
using Services.Tournaments;

public class MatchesController : BaseGatewayController
{
    private readonly IMatchesService _matches;

    public MatchesController(IMatchesService matches, ILogger<MatchesController> logger) 
        : base(logger) 
        => _matches = matches;

    [HttpGet]
    [Route(nameof(All))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> All()
        => await SafeProcessRefitRequest(
            async () =>
            {
                var matches = await _matches.All();
                return Ok(matches);
            }, "An error occurred while trying to get all matches");
    
    [HttpGet]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchShortOutputModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ById(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var match = await _matches.ById(id);
                return Ok(match);
            }, $"An error occurred during GET request for match: {id}");
    
    [HttpGet]
    [Route($"{nameof(GetOrganiserUsername)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrganiserUsername(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var matchTournamentOrganiserUsername = await _matches.GetOrganiserUsername(id);
                return Ok(matchTournamentOrganiserUsername);
            }, $"An error occurred during GET request for match organiser username for match: {id}");
    
    [HttpGet]
    [Route(nameof(Search))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchOutputModel<MatchOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] MatchQuery query)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var matches = await _matches.Search(query);
                return Ok(matches);
            }, $"An error occurred during matches search with query: {query}");
    
    [HttpPost]
    [Route($"{nameof(AddPeriodInfo)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPeriodInfo(int id, [FromBody] MatchResultsInputModel matchResultsInputModel)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _matches.AddPeriodInfo(id, matchResultsInputModel);
                return Ok(result);
            }, $"An error occurred during AddPeriodInfo request for match {id}. Request: {matchResultsInputModel}");
    
    
    [HttpPost]
    [Route($"{nameof(UpdateMatchOutcomeDueToCustomCondition)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMatchOutcomeDueToCustomCondition(int id, [FromBody] MatchCustomConditionResultInputModel matchCustomConditionResultInput)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _matches.UpdateMatchOutcomeDueToCustomCondition(id, matchCustomConditionResultInput);
                return Ok(result);
            }, $"An error occurred during {nameof(UpdateMatchOutcomeDueToCustomCondition)} request for match {id}. Request: {matchCustomConditionResultInput}");

    [HttpDelete]
    [Route($"{nameof(DeleteMatchPeriods)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMatchPeriods(int id)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _matches.DeleteMatchPeriods(id);
                return Ok(result);
            }, $"An error occurred during DeleteMatchPeriods request for match {id}.");
    
    [HttpDelete]
    [Route($"{nameof(DeleteMatchPeriodsAfterSetAndGameInclusive)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMatchPeriodsAfterSetAndGameInclusive(int id, int set, int game)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _matches.DeleteMatchPeriodsAfterSetAndGameInclusive(id, set, game);
                return Ok(result);
            }, $"An error occurred during DeleteMatchPeriodsAfterSetAndGameInclusive request for match {id}. Set: {set}, Game: {game}.");
    
    [HttpPost]
    [Route(nameof(Add))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add(MatchInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var tournamentId = await _matches.Create(input);
                return Ok(tournamentId);
            }, $"An error occurred during Create request for match. Request: {input}");
    
    [HttpPut]
    [Route($"{nameof(Edit)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(int id, MatchInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _matches.Edit(id, input);
                return Ok(result);
            }, $"An error occurred during Edit request for match: {id}. Request: {input}");
    
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
                var result = await _matches.Delete(id);
                return Ok(result);
            }, $"An error occurred during Delete request for match: {id}");
}