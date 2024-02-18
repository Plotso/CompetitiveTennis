namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Contracts.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Match;
using Services;
using Services.Interfaces.Data;

public class MatchesController : ApiController
{
    private readonly IMatchesService _matches;
    private readonly IAccountsService _accounts;
    private readonly ITournamentsService _tournaments;
    private readonly IParticipantsService _participants;
    private readonly ICurrentUserService _currentUser;

    public MatchesController(
        IMatchesService matches,
        IAccountsService accounts,
        ITournamentsService tournaments,
        IParticipantsService participants,
        ICurrentUserService currentUser,
        ILogger<MatchesController> logger) : base(logger)
    {
        _matches = matches;
        _accounts = accounts;
        _tournaments = tournaments;
        _participants = participants;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Route(nameof(All))]
    public async Task<ActionResult<Result<IEnumerable<MatchOutputModel>>>> All()
        => await SafeHandle(async () =>
            {
                var matches = await _matches.GetAll();
                return Ok(Result<IEnumerable<MatchOutputModel>>.SuccessWith(matches));
            },
            msgOnError: "An error occurred during GET request for all matches");

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<MatchShortOutputModel>> ById(int id)
        => await SafeHandle(async () =>
            {
                var tournamentId = await _matches.GetTournamentIdForMatch(id);
                if (tournamentId == null)
                    return NotFound(Result.Failure($"Match {id} does not exist"));
                
                var tournament = await _tournaments.Get(tournamentId.Value);
                var match = MatchInfoProvider.GetMatchInfoFromTournament(tournament, id);
                return Ok(Result<MatchShortOutputModel>.SuccessWith(match));
            },
            msgOnError: $"An error occurred during GET request for match: {id}");
    
    [HttpGet]
    [Route($"{nameof(GetOrganiserUsername)}/{Id}")]
    public async Task<ActionResult<string>> GetOrganiserUsername(int id)
        => await SafeHandle(async () =>
            {
                var tournamentId = await _matches.GetTournamentIdForMatch(id);
                if (tournamentId == null)
                    return NotFound(Result.Failure($"Match {id} does not exist"));
                var tournamentOrganiserUsername = await _tournaments.GetOrganiserUsername(tournamentId.Value);
                return Ok(Result<string>.SuccessWith(tournamentOrganiserUsername));
            },
            msgOnError: $"An error occurred during GET request for tournament organiser name for tournament: {id}");

    [HttpPost]
    [Route(nameof(Add))]
    [Authorize]
    public async Task<ActionResult<Result<int>>> Add(MatchInputModel input)
        => await SafeHandle(async () =>
            {
                var organiser = await _accounts.GetByUserId(_currentUser.UserId);
                if (organiser == null)
                {
                    Logger.LogCritical($"{_currentUser.UserId} lacks account in internal system");
                    return BadRequest(
                        Result.Failure("Current user account has lacking data preventing organisation of events. Please send support request."));
                }

                if (input.EndDate < input.StartDate)
                    return BadRequest(Result.Failure("Match cannot have end date that is before the start date."));

                var tournament = await _tournaments.GetInternal(input.TournamentId);
                if (tournament != null)
                    return BadRequest(Result.Failure($"Match cannot be added to missing tournament. TournamentId: {input.TournamentId}."));

                var homeParticipant = await _participants.GetInternal(input.Participant1Id);
                if (homeParticipant != null && homeParticipant.TournamentId != tournament.Id)
                    return BadRequest(Result.Failure($"Home participant is linked to different tournament than the one in the request. ParticipantTournament: {homeParticipant.TournamentId}. Request TournamentId: {input.TournamentId}."));

                var awayParticipant = await _participants.GetInternal(input.Participant1Id);
                if (awayParticipant != null && awayParticipant.TournamentId != tournament.Id)
                    return BadRequest(Result.Failure($"Away participant is linked to different tournament than the one in the request. ParticipantTournament: {homeParticipant.TournamentId}. Request TournamentId: {input.TournamentId}."));
                var matchId = await _matches.Create(input, tournament, homeParticipant, awayParticipant);
                return Ok(Result<int>.SuccessWith(matchId));
            },
            msgOnError: $"Unexpected error during match creation. MatchInput: {input}");

    [HttpPut]
    [Route($"{nameof(Edit)}/{Id}")]
    [Authorize]
    public async Task<ActionResult> Edit(int id, MatchInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(input.TournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to update matches from respective tournament!"));
                if (input.EndDate < input.StartDate)
                    return BadRequest(Result.Failure("Match cannot have end date that is before the start date."));

                var isSuccess = await _matches.Update(id, input);
                return isSuccess ? Result.Success : Result.Failure($"Update for match {id} failed.");
            },
            msgOnError: $"Unexpected error during match update. MatchInput: {input}. MatchId: {id}");


    [HttpDelete]
    [Route(Id)]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
        => await SafeHandle(async () =>
            {
                var match = await _matches.GetInternal(id);
                if (match == null)
                    return Result.Success;
            
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(match.TournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to delete matches from tournament!"));

                var isSuccess = await _matches.Delete(id, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Delete operation for match {id} failed.");
            },
            msgOnError: $"Unexpected error during match delete. MatchId: {id}");


    private async Task<bool> IsCurrentUserOrganiser(int tournamentId) 
        => await _tournaments.GetOrganiserUserId(tournamentId) == _currentUser.UserId;

}
