namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using CompetitiveTennis.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Participant;
using Models.Tournament;
using Services.Tournaments;

public class TournamentsController : BaseGatewayController
{
    private readonly ITournamentsService _tournaments;

    public TournamentsController(ITournamentsService tournaments, ILogger<TournamentsController> logger) 
        : base(logger) 
        => _tournaments = tournaments;

    [HttpGet]
    [Route(nameof(All))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TournamentOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> All()
        => await SafeProcessRefitRequest(
            async () =>
            {
                var tournaments = await _tournaments.All();
                return Ok(tournaments);
            }, "An error occurred while trying to get all tournaments");
    
    [HttpGet]
    [Route(Id)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TournamentOutputModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ById(int id) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var tournament = await _tournaments.ById(id);
                return Ok(tournament);
            }, $"An error occurred during GET request for tournament: {id}");
    
    [HttpGet]
    [Route(nameof(Search))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchOutputModel<TournamentOutputModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] TournamentQuery query)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var tournaments = await _tournaments.Search(query);
                return Ok(tournaments);
            }, $"An error occurred during tournaments search with query: {query}");
    
    [HttpPost]
    [Route(nameof(Add))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add(TournamentInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var tournamentId = await _tournaments.Create(input);
                return Ok(tournamentId);
            }, $"An error occurred during Create request for tournament. Request: {input}");
    
    [HttpPut]
    [Route($"{nameof(Edit)}/{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(int id, TournamentInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.Edit(id, input);
                return Ok(result);
            }, $"An error occurred during Edit request for tournament: {id}. Request: {input}");
    
    [HttpPut]
    [Route(nameof(ChangeAvenue))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeAvenue(int tournamentId, int newAvenueId) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.ChangeAvenue(tournamentId, newAvenueId);
                return Ok(result);
            }, $"An error occurred during ChangeAvenue request for tournament: {tournamentId}, newAvenueId: {newAvenueId}");
    
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
                var result = await _tournaments.Delete(id);
                return Ok(result);
            }, $"An error occurred during Delete request for tournament: {id}");
    
    [HttpPost]
    [Route(nameof(AddGuest))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddGuest(ParticipantInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.AddGuest(input);
                return Ok(result);
            }, $"An error occurred during AddGuest request for tournament. Request: {input}");
    
    [HttpPost]
    [Route(nameof(ParticipateSingle))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ParticipateSingle(ParticipantInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.ParticipateSingle(input);
                return Ok(result);
            }, $"An error occurred during ParticipateSingle request for tournament. Request: {input}");
    
    [HttpPost]
    [Route(nameof(ParticipateDoubles))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ParticipateDoubles(MultiParticipantInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.ParticipateDoubles(input);
                return Ok(result);
            }, $"An error occurred during ParticipateDoubles request for tournament. Request: {input}");
    
    [HttpPost]
    [Route(nameof(AddSinglesParticipant))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddSinglesParticipant(AccountParticipantInputModel input) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.AddSinglesParticipant(input);
                return Ok(result);
            }, $"An error occurred during AddSinglesParticipant request for tournament. Request: {input}");
    
    [HttpPost]
    [Route(nameof(AddAccountToParticipant))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddAccountToParticipant(int tournamentId, int participantId, int accountId) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.AddAccountToParticipant(tournamentId, participantId, accountId);
                return Ok(result);
            }, $"An error occurred during AddAccountToParticipant request for tournament: {tournamentId}, participant: {participantId}, account : {accountId}");
    
    [HttpPost]
    [Route(nameof(RemoveAccountFromParticipant))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAccountFromParticipant(int tournamentId, int participantId, int accountId) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.RemoveAccountFromParticipant(tournamentId, participantId, accountId);
                return Ok(result);
            }, $"An error occurred during RemoveAccountFromParticipant request for tournament: {tournamentId}, participant: {participantId}, account : {accountId}");
    
    [HttpPost]
    [Route(nameof(RemoveParticipantFromTournament))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveParticipantFromTournament(int tournamentId, int participantId) 
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.RemoveParticipantFromTournament(tournamentId, participantId);
                return Ok(result);
            }, $"An error occurred during RemoveParticipantFromTournament request for tournament: {tournamentId}, participant: {participantId}");
    
    
    [HttpPost]
    [Route(nameof(GenerateTournamentDraw))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateTournamentDraw(int tournamentId)
        => await SafeProcessRefitRequest(
            async () =>
            {
                var result = await _tournaments.GenerateTournamentDraw(tournamentId);
                return Ok(result);
            }, $"An error occurred during GenerateTournamentDraw request for tournament: {tournamentId}");
}