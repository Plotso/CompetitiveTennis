﻿namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Participant;
using Models.Tournament;
using Services.Interfaces;

public class TournamentsController : ApiController
{
    private readonly ITournamentsService _tournaments;
    private readonly IAvenuesService _avenues;
    private readonly IAccountsService _accounts;
    private readonly IParticipantsService _participants;
    private readonly ITeamsService _teams;
    private readonly ICurrentUserService _currentUser;

    public TournamentsController(
        ITournamentsService tournaments,
        IAvenuesService avenues,
        IAccountsService accounts,
        IParticipantsService participants,
        ITeamsService teams,
        ICurrentUserService currentUser,
        ILogger<TournamentsController> logger) : base(logger)
    {
        _tournaments = tournaments;
        _avenues = avenues;
        _accounts = accounts;
        _participants = participants;
        _teams = teams;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Route(nameof(All))]
    public async Task<ActionResult<IEnumerable<TournamentOutputModel>>> All()
        => await SafeHandle(async () =>
            {
                var tournaments = await _tournaments.GetAll();
                return Ok(tournaments);
            },
            msgOnError: "An error occured during GET request for all tournaments");

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<IEnumerable<TournamentOutputModel>>> ById(int id)
        => await SafeHandle(async () =>
            {
                var tournament = await _tournaments.Get(id);
                if (tournament == null)
                    return NotFound($"Tournament {id} does not exist");
                return Ok(tournament);
            },
            msgOnError: $"An error occured during GET request for tournament: {id}");

    [HttpGet]
    [Route(nameof(Search))]
    public async Task<ActionResult<SearchOutputModel<TournamentOutputModel>>> Search([FromQuery] TournamentQuery query)
        => await SafeHandle(async () =>
            {
                var tournaments = await _tournaments.Query(query);
                var total = await _tournaments.Total(query);
                return Ok(new SearchOutputModel<TournamentOutputModel>(tournaments, query.Page, total));
            },
            msgOnError: $"An error occured during Search request with query: {query}");

    [HttpPost]
    [Route(nameof(Add))]
    [Authorize]
    public async Task<ActionResult<int>> Add(TournamentInputModel input)
        => await SafeHandle(async () =>
            {
                var organiser = await _accounts.GetByUserId(_currentUser.UserId);
                if (organiser == null)
                {
                    Logger.LogCritical($"{_currentUser.UserId} lacks account in internal system");
                    return BadRequest(
                        Result.Failure("Current user account has lacking data preventing organisation of events. Please send support request."));
                }

                var avenue = await _avenues.GetInternal(input.AvenueId);
                if (avenue == null)
                    return BadRequest(Result.Failure("Provided avenue could not be found in internal system."));

                var tournamentId = await _tournaments.Create(input, organiser, avenue);
                return Ok(tournamentId);
            },
            msgOnError: $"Unexpected error during tournament creation. TournamentInput: {input}");

    [HttpPut]
    [Route($"{nameof(Edit)}/{Id}")]
    [Authorize]
    public async Task<ActionResult> Edit(int id, TournamentInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(id) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to update tournament!"));

                var isSuccess = await _tournaments.Update(id, input);
                return isSuccess ? Result.Success : Result.Failure($"Update for tournament {id} failed.");
            },
            msgOnError: $"Unexpected error during tournament update. TournamentInput: {input}. TournamentId: {id}");

    [HttpPut]
    [Route(nameof(ChangeAvenue))]
    [Authorize]
    public async Task<ActionResult> ChangeAvenue(int tournamentId, int newAvenueId)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(tournamentId) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to update tournament!"));

                var isSuccess = await _tournaments.ChangeAvenue(tournamentId, tournamentId);
                return isSuccess ? Result.Success : Result.Failure($"Update avenue for tournament {tournamentId} failed.");
            },
            msgOnError: $"Unexpected error during tournament avenue update. TournamentId: {tournamentId}. NewAvenueId: {newAvenueId}",
            msgOnNotFound: $"{nameof(ChangeAvenue)} failed due to missing avenue with id {newAvenueId}");

    [HttpDelete]
    [Route(Id)]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(id) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to delete tournament!"));

                var isSuccess = await _avenues.Delete(id, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Delete operation for tournament {id} failed.");
            },
            msgOnError: $"Unexpected error during tournament delete. TournamentId: {id}");

    [HttpPost]
    [Route(nameof(AddGuest))]
    [Authorize]
    public async Task<ActionResult<int>> AddGuest(ParticipantInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(input.TournamentId) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to add guests to tournament!"));
                if (string.IsNullOrWhiteSpace(input.Name))
                    return BadRequest("Name is mandatory for guest participants");
                
                var team = input.TeamId.HasValue ? await _teams.GetInternal(input.TeamId.Value) : null;
                var tournament = await _tournaments.GetInternal(input.TournamentId);
                if (tournament == null)
                    return BadRequest(Result.Failure($"Tournament {input.TournamentId} could not be found!"));
                
                var participantId = await _participants.Create(input, tournament, team);
                return Ok(participantId);
            },
            msgOnError: $"Unexpected error during guest participant creation. ParticipantInput: {input}");


    [HttpPost]
    [Route(nameof(ParticipateSingle))]
    [Authorize]
    public async Task<ActionResult> ParticipateSingle(ParticipantInputModel input)
        => await SafeHandle(async () =>
            {
                var team = input.TeamId.HasValue ? await _teams.GetInternal(input.TeamId.Value) : null;
                var tournament = await _tournaments.GetInternal(input.TournamentId);
                if (tournament == null)
                    return BadRequest(Result.Failure($"Tournament {input.TournamentId} could not be found!"));
                if (tournament.Type != TournamentType.Singles)
                    return BadRequest(
                        Result.Failure("Registration for doubles & teams tournaments are handled separately!"));
                
                var participantId = await _participants.Create(input, tournament, team);
                var currentAccount = await _accounts.GetByUserId(_currentUser.UserId);
                var isSuccess = await _participants.AddUsersToParticipant(participantId, new List<Account> {currentAccount});
                return isSuccess ? Result.Success : Result.Failure($"Failed to add currentUser to participant {participantId}");
            },
            msgOnError: $"Unexpected error during guest participant creation. ParticipantInput: {input}");


    [HttpPost]
    [Route(nameof(ParticipateDoubles))]
    [Authorize]
    public async Task<ActionResult> ParticipateDoubles(MultiParticipantInputModel input)
        => await SafeHandle(async () =>
            {
                var team = input.ParticipantInfo.TeamId.HasValue ? await _teams.GetInternal(input.ParticipantInfo.TeamId.Value) : null;
                var tournament = await _tournaments.GetInternal(input.ParticipantInfo.TournamentId);
                if (tournament == null)
                    return BadRequest(Result.Failure($"Tournament {input.ParticipantInfo.TournamentId} could not be found!"));
                if (tournament.Type != TournamentType.Doubles)
                    return BadRequest(
                        Result.Failure("Registration for singles & teams tournaments are handled separately!"));

                if (input.Accounts.Count() != 2 && (!input.ParticipantInfo.IsGuest || !input.IncludeCurrentUser))
                    return BadRequest(Result.Failure("Registration for doubles participation requires 2 participants"));
                
                if (input.ParticipantInfo.IsGuest && string.IsNullOrWhiteSpace(input.ParticipantInfo.Name))
                    return BadRequest("Name is mandatory for guest participants");
                
                var participantId = await _participants.Create(input.ParticipantInfo, tournament, team);
                var currentUserAccount = await _accounts.GetByUserId(_currentUser.UserId);
                var accounts = await _accounts.GetMultiple(input.Accounts);
                if (accounts.Count() != input.Accounts.Count())
                {
                    Logger.LogCritical($"Some of the following accounts are missing from internal system: {string.Join(" ", input.Accounts)}");
                    return BadRequest("Not all accounts could be found in internal system");
                }
                var accsToSign = accounts.ToList();
                if (input.IncludeCurrentUser && accsToSign.All(a => a.UserId != _currentUser.UserId))
                {
                    if (currentUserAccount == null)
                    {
                        Logger.LogCritical($"{_currentUser.UserId} lacks account in internal system");
                        return BadRequest("Current user account has lacking data preventing participations");
                    }

                    accsToSign.Add(currentUserAccount);
                }
                var isSuccess = await _participants.AddUsersToParticipant(participantId, accsToSign);
                return isSuccess ? Result.Success : Result.Failure($"Failed to add currentUser to participant {participantId}");
            },
            msgOnError: $"Unexpected error during guest participant creation. ParticipantInput: {input}");
    
    [HttpPost]
    [Route(nameof(AddAccountToParticipant))]
    [Authorize]
    public async Task<ActionResult<bool>> AddAccountToParticipant(int tournamentId, int participantId, int accountId)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(tournamentId) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to add accounts to existing participants!"));
                if (await _participants.IsParticipantFull(participantId))
                    return BadRequest(
                        Result.Failure($"No more players can be added to participant {participantId} since it has reached max cap for tournament type"));
                
                
                var account = await _accounts.GetInternal(accountId);
                if (account == null)
                    return BadRequest($"Account {accountId} could not be found in internal system");

                var isSuccess = await _participants.AddUsersToParticipant(participantId, new[] {account});
                return isSuccess ? Result.Success : Result.Failure($"Failed to add account {accountId} to participant {participantId}");
            },
            msgOnError: $"Unexpected error during account addition to participant. ParticipantId: {participantId}. AccountId: {accountId}");
    
    [HttpPost]
    [Route(nameof(RemoveAccountFromParticipant))]
    [Authorize]
    public async Task<ActionResult<bool>> RemoveAccountFromParticipant(int tournamentId, int participantId, int accountId)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(tournamentId) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser)
                    return BadRequest(
                        Result.Failure("Only admins and tournament organiser are allowed to remove accounts to existing participants!"));

                var account = await _accounts.GetInternal(accountId);
                if (account == null)
                    return BadRequest($"Account {accountId} could not be found in internal system");

                var isSuccess = await _participants.RemoveUsersFromParticipant(participantId, new[] {account});
                return isSuccess ? Result.Success : Result.Failure($"Failed to remove account {accountId} from participant {participantId}");
            },
            msgOnError: $"Unexpected error during account removal to participant. ParticipantId: {participantId}. AccountId: {accountId}");
    
    
    [HttpPost]
    [Route(nameof(RemoveParticipantFromTournament))]
    [Authorize]
    public async Task<ActionResult<bool>> RemoveParticipantFromTournament(int tournamentId, int participantId)
        => await SafeHandle(async () =>
            {
                var participant = await _participants.GetInternal(participantId);
                if (participant == null)
                    return BadRequest(Result.Failure($"Participant {participantId} could not be found."));
                if (participant.TournamentId == tournamentId && participant.Matches.Any())
                    return BadRequest(Result.Failure("Participant with existing matches cannot be removed from a tournament"));
                var isCurrentUserParticipant = participant.Players.Any(p => p.Account.UserId == _currentUser.UserId);
                var isCurrentUserOrganiser = await _tournaments.GetOrganiserUserId(tournamentId) != _currentUser.UserId;
                if (!_currentUser.IsAdministrator || !isCurrentUserOrganiser || !isCurrentUserParticipant)
                    return BadRequest(
                        Result.Failure("Only admins, tournament organiser and participant accounts are allowed to remove participant from existing competition!"));


                var isSuccess = await _tournaments.RemoveParticipant(tournamentId, participant);
                return isSuccess ? Result.Success : Result.Failure($"Failed to remove participant {participantId} from tournament {tournamentId}");
            },
            msgOnError: $"Unexpected error during participant removal from tournament. ParticipantId: {participantId}. TournamentId: {tournamentId}");

}