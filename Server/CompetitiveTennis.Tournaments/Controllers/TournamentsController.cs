namespace CompetitiveTennis.Tournaments.Controllers;

using CompetitiveTennis.Controllers;
using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Models;
using CompetitiveTennis.Services.Interfaces;
using Contracts;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contracts.Participant;
using Contracts.Tournament;
using Models.TournamentDrawGenerator;
using Services;
using Services.Interfaces.BL;
using Services.Interfaces.Data;

public class TournamentsController : ApiController
{
    private readonly ITournamentsService _tournaments;
    private readonly IAvenuesService _avenues;
    private readonly IAccountsService _accounts;
    private readonly IParticipantsService _participants;
    private readonly ITeamsService _teams;
    private readonly ITournamentDrawGenerator _tournamentDrawGenerator;
    private readonly ICurrentUserService _currentUser;

    public TournamentsController(
        ITournamentsService tournaments,
        IAvenuesService avenues,
        IAccountsService accounts,
        IParticipantsService participants,
        ITeamsService teams,
        ITournamentDrawGenerator tournamentDrawGenerator,
        ICurrentUserService currentUser,
        ILogger<TournamentsController> logger) : base(logger)
    {
        _tournaments = tournaments;
        _avenues = avenues;
        _accounts = accounts;
        _participants = participants;
        _teams = teams;
        _tournamentDrawGenerator = tournamentDrawGenerator;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Route(nameof(All))]
    public async Task<ActionResult<Result<IEnumerable<TournamentOutputModel>>>> All()
        => await SafeHandle(async () =>
            {
                var tournaments = await _tournaments.GetAll();
                return Ok(Result<IEnumerable<TournamentOutputModel>>.SuccessWith(tournaments));
            },
            msgOnError: "An error occurred during GET request for all tournaments");

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<SlimTournamentOutputModel>> ById(int id)
        => await SafeHandle(async () =>
            {
                var tournament = await _tournaments.Get(id);
                if (tournament == null)
                    return NotFound(Result.Failure($"Tournament {id} does not exist"));
                return Ok(Result<SlimTournamentOutputModel>.SuccessWith(TournamentInfoProvider.GetTournamentInfo(tournament)));
            },
            msgOnError: $"An error occurred during GET request for tournament: {id}");
    
    [HttpGet]
    [Route($"{nameof(GetTournamentName)}/{Id}")]
    public async Task<ActionResult<string>> GetTournamentName(int id)
        => await SafeHandle(async () =>
            {
                var tournamentName = await _tournaments.GetTournamentName(id);
                if (tournamentName == null)
                    return NotFound(Result.Failure($"Tournament {id} does not exist"));
                return Ok(Result<string>.SuccessWith(tournamentName));
            },
            msgOnError: $"An error occurred during GET request for tournament name for tournament: {id}");
    
    [HttpGet]
    [Route($"{nameof(GetOrganiserUsername)}/{Id}")]
    public async Task<ActionResult<string>> GetOrganiserUsername(int id)
        => await SafeHandle(async () =>
            {
                var tournamentOrganiserUsername = await _tournaments.GetOrganiserUsername(id);
                if (tournamentOrganiserUsername == null)
                    return NotFound(Result.Failure($"Tournament {id} does not exist"));
                return Ok(Result<string>.SuccessWith(tournamentOrganiserUsername));
            },
            msgOnError: $"An error occurred during GET request for tournament organiser name for tournament: {id}");

    [HttpGet]
    [Route(nameof(Search))]
    public async Task<ActionResult<Result<SearchOutputModel<TournamentOutputModel>>>> Search([FromQuery] TournamentQuery query)
        => await SafeHandle(async () =>
            {
                var tournaments = await _tournaments.Query(query);
                var total = await _tournaments.Total(query);
                return Ok(Result<SearchOutputModel<TournamentOutputModel>>.SuccessWith(
                    new SearchOutputModel<TournamentOutputModel>(tournaments, query.Page, total)));
            },
            msgOnError: $"An error occurred during Search request with query: {query}");

    [HttpPost]
    [Route(nameof(Add))]
    [Authorize]
    public async Task<ActionResult<Result<int>>> Add(TournamentInputModel input)
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

                if (input.EndDate < input.StartDate)
                    return BadRequest(Result.Failure("Tournament cannot have end date that is before the start date."));

                var tournamentId = await _tournaments.Create(input, organiser, avenue);
                return Ok(Result<int>.SuccessWith(tournamentId));
            },
            msgOnError: $"Unexpected error during tournament creation. TournamentInput: {input}");

    [HttpPut]
    [Route($"{nameof(Edit)}/{Id}")]
    [Authorize]
    public async Task<ActionResult> Edit(int id, TournamentInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(id);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to update tournament!"));
                if (input.EndDate < input.StartDate)
                    return BadRequest(Result.Failure("Tournament cannot have end date that is before the start date."));

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
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(tournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
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
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(id);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to delete tournament!"));

                var isSuccess = await _tournaments.Delete(id, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Delete operation for tournament {id} failed.");
            },
            msgOnError: $"Unexpected error during tournament delete. TournamentId: {id}");

    [HttpPost]
    [Route(nameof(AddGuest))]
    [Authorize]
    public async Task<ActionResult<Result<int>>> AddGuest(ParticipantInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(input.TournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to add guests to tournament!"));
                if (string.IsNullOrWhiteSpace(input.Name))
                    return BadRequest(Result.Failure("Name is mandatory for guest participants"));
                
                var team = input.TeamId.HasValue ? await _teams.GetInternal(input.TeamId.Value) : null;
                var tournament = await _tournaments.GetInternal(input.TournamentId);
                if (tournament == null)
                    return BadRequest(Result.Failure($"Tournament {input.TournamentId} could not be found!"));
                if (tournament.Matches.Any())
                {
                    Logger.LogInformation($"An attempt to add guest to an ongoing tournament has been made. TournamentId: {input.TournamentId}. Endpoint: {nameof(AddGuest)}. Input: {input}");
                    return BadRequest(Result.Failure($"Cannot add guest to an ongoing tournament. Tournament {input.TournamentId}."));
                }
                
                var participantId = await _participants.Create(input, tournament, team);
                return Ok(Result<int>.SuccessWith(participantId));
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
                if (tournament.Matches.Any())
                {
                    Logger.LogInformation($"An attempt to participate in an ongoing tournament has been made. TournamentId: {input.TournamentId}. Endpoint: {nameof(ParticipateSingle)}. Input: {input}");
                    return BadRequest(Result.Failure($"Cannot participate in an ongoing tournament. Tournament {input.TournamentId}."));
                }
                if (tournament.Type != TournamentType.Singles)
                    return BadRequest(
                        Result.Failure("Registration for doubles & teams tournaments are handled separately!"));
                
                var currentAccount = await _accounts.GetByUserId(_currentUser.UserId);
                if (await _tournaments.IsAccountPresentInAnyParticipant(currentAccount.Id, input.TournamentId))
                    return Result.Success;
                
                var participantId = await _participants.Create(input, tournament, team);
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
                if (tournament.Matches.Any())
                {
                    Logger.LogInformation($"An attempt to participate in an ongoing tournament has been made. TournamentId: {input.ParticipantInfo.TournamentId}. Endpoint: {nameof(ParticipateDoubles)}. Input: {input}");
                    return BadRequest(Result.Failure($"Cannot participate in an ongoing tournament. Tournament {input.ParticipantInfo.TournamentId}."));
                }
                if (tournament.Type != TournamentType.Doubles)
                    return BadRequest(
                        Result.Failure("Registration for singles & teams tournaments are handled separately!"));

                if (input.Accounts.Count() != 2 && (!input.ParticipantInfo.IsGuest && !input.IncludeCurrentUser))
                    return BadRequest(Result.Failure("Registration for doubles participation requires 2 participants"));
                
                if (input.ParticipantInfo.IsGuest && string.IsNullOrWhiteSpace(input.ParticipantInfo.Name))
                    return BadRequest(Result.Failure("Name is mandatory for guest participants"));
                var registeredAccountsForTournament = await _tournaments.GetRegisteredAccountsForTournament(input.ParticipantInfo.TournamentId);
                if (input.Accounts.Any(a => registeredAccountsForTournament.Contains(a)))
                    return BadRequest(
                        Result.Failure("One or more of the provided accounts is part of another participant!"));
                var participantId = await _participants.Create(input.ParticipantInfo, tournament, team);
                var currentUserAccount = await _accounts.GetByUserId(_currentUser.UserId);
                var accounts = await _accounts.GetMultiple(input.Accounts);
                if (accounts.Count() != input.Accounts.Count(id => id > 0))
                {
                    Logger.LogCritical($"Some of the following accounts are missing from internal system: {string.Join(" ", input.Accounts)}");
                    return BadRequest(Result.Failure("Not all accounts could be found in internal system"));
                }
                var accsToSign = accounts.ToList();
                if (input.IncludeCurrentUser && accsToSign.All(a => a.UserId != _currentUser.UserId))
                {
                    if (currentUserAccount == null)
                    {
                        Logger.LogCritical($"{_currentUser.UserId} lacks account in internal system");
                        return BadRequest(Result.Failure("Current user account has lacking data preventing participations"));
                    }

                    accsToSign.Add(currentUserAccount);
                }

                foreach (var accountId in accsToSign.Select(a => a.Id))
                {
                    if (await _tournaments.IsAccountPresentInAnyParticipant(accountId, input.ParticipantInfo.TournamentId))
                        return BadRequest(
                            Result.Failure($"Account {accountId} is already part of a participant for tournament {input.ParticipantInfo.TournamentId}"));
                }
                var isSuccess = await _participants.AddUsersToParticipant(participantId, accsToSign);
                return isSuccess ? Result.Success : Result.Failure($"Failed to add currentUser to participant {participantId}");
            },
            msgOnError: $"Unexpected error during guest participant creation. ParticipantInput: {input}");


    [HttpPost]
    [Route(nameof(AddSinglesParticipant))]
    [Authorize]
    public async Task<ActionResult<Result>> AddSinglesParticipant(AccountParticipantInputModel input)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(input.ParticipantInput.TournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to add other singles participants!"));

                if (input.AccountId <= 0)
                    return BadRequest(
                        Result.Failure("AccountId must be greater than 0!"));
                
                var currentAccount = await _accounts.GetInternal(input.AccountId);
                if (currentAccount == null)
                    return BadRequest(Result.Failure("Account with provided id could not be found!"));

                if (await _tournaments.IsAccountPresentInAnyParticipant(currentAccount.Id, input.ParticipantInput.TournamentId))
                    return Result.Success;
                
                var team = input.ParticipantInput.TeamId.HasValue ? await _teams.GetInternal(input.ParticipantInput.TeamId.Value) : null;
                var tournament = await _tournaments.GetInternal(input.ParticipantInput.TournamentId);
                if (tournament == null)
                    return BadRequest(Result.Failure($"Tournament {input.ParticipantInput.TournamentId} could not be found!"));
                if (tournament.Matches.Any())
                {
                    Logger.LogInformation($"An attempt to add a participant to an ongoing tournament has been made. TournamentId: {input.ParticipantInput.TournamentId}. Endpoint: {nameof(AddSinglesParticipant)}. Input: {input}");
                    return BadRequest(Result.Failure($"Cannot add a participant to an ongoing in an ongoing tournament. Tournament {input.ParticipantInput.TournamentId}."));
                }
                if (tournament.Type != TournamentType.Singles)
                    return BadRequest(
                        Result.Failure("Registration for doubles & teams tournaments are handled separately!"));
                
                var participantId = await _participants.Create(input.ParticipantInput, tournament, team);
                var isSuccess = await _participants.AddUsersToParticipant(participantId, new List<Account> {currentAccount});
                return isSuccess ? Result.Success : Result.Failure($"Failed to add singles participant");
            },
            msgOnError: $"Unexpected error during singles participant addition. AccountParticipantInput: {input}.");
    
    [HttpPost]
    [Route(nameof(AddAccountToParticipant))]
    [Authorize]
    public async Task<ActionResult<Result>> AddAccountToParticipant(int tournamentId, int participantId, int accountId)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(tournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to add accounts to existing participants!"));
                if (await _tournaments.HasTournamentAlreadyStarted(tournamentId))
                {
                    Logger.LogInformation($"An attempt to add an account to a participant for an ongoing tournament has been made. TournamentId: {tournamentId}. Endpoint: {nameof(AddAccountToParticipant)}. ParticipantId: {participantId}. AccountId: {accountId}");
                    return BadRequest(Result.Failure($"Cannot add an account to a participant for an ongoing in an ongoing tournament. Tournament {tournamentId}."));
                }
                if (await _participants.IsParticipantFull(participantId))
                    return BadRequest(
                        Result.Failure($"No more players can be added to participant {participantId} since it has reached max cap for tournament type"));

                if (await _tournaments.IsAccountPresentInAnyParticipant(accountId, tournamentId))
                    return BadRequest(
                        Result.Failure($"Account {accountId} is already part of a participant for tournament {tournamentId}"));
                
                
                var account = await _accounts.GetInternal(accountId);
                if (account == null)
                    return BadRequest(Result.Failure($"Account {accountId} could not be found in internal system"));

                var isSuccess = await _participants.AddUsersToParticipant(participantId, new[] {account});
                return isSuccess ? Result.Success : Result.Failure($"Failed to add account {accountId} to participant {participantId}");
            },
            msgOnError: $"Unexpected error during account addition to participant. ParticipantId: {participantId}. AccountId: {accountId}");
    
    [HttpPost]
    [Route(nameof(RemoveAccountFromParticipant))]
    [Authorize]
    public async Task<ActionResult<Result>> RemoveAccountFromParticipant(int tournamentId, int participantId, int accountId)
        => await SafeHandle(async () =>
            {
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(tournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                    return Unauthorized(
                        Result.Failure("Only admins and tournament organiser are allowed to remove accounts to existing participants!"));
                if (await _tournaments.HasTournamentAlreadyStarted(tournamentId))
                {
                    Logger.LogInformation($"An attempt to remove an account from a participant for an ongoing tournament has been made. TournamentId: {tournamentId}. Endpoint: {nameof(RemoveAccountFromParticipant)}. ParticipantId: {participantId}. AccountId: {accountId}");
                    return BadRequest(Result.Failure($"Cannot remove an account from a participant for an ongoing in an ongoing tournament. Tournament {tournamentId}."));
                }

                var account = await _accounts.GetInternal(accountId);
                if (account == null)
                    return BadRequest(Result.Failure($"Account {accountId} could not be found in internal system"));

                var isSuccess = await _participants.RemoveUsersFromParticipant(participantId, new[] {account});
                return isSuccess ? Result.Success : Result.Failure($"Failed to remove account {accountId} from participant {participantId}");
            },
            msgOnError: $"Unexpected error during account removal to participant. ParticipantId: {participantId}. AccountId: {accountId}");
    
    
    [HttpPost]
    [Route(nameof(RemoveParticipantFromTournament))]
    [Authorize]
    public async Task<ActionResult<Result>> RemoveParticipantFromTournament(int tournamentId, int participantId)
        => await SafeHandle(async () =>
            {
                if (await _tournaments.HasTournamentAlreadyStarted(tournamentId))
                {
                    Logger.LogInformation($"An attempt to remove a participant from an ongoing tournament has been made. TournamentId: {tournamentId}. Endpoint: {nameof(RemoveParticipantFromTournament)}. ParticipantId: {participantId}.");
                    return BadRequest(Result.Failure($"Cannot remove a participant from an ongoing in an ongoing tournament. Tournament {tournamentId}."));
                }
                var participant = await _participants.GetInternal(participantId);
                if (participant == null)
                    return BadRequest(Result.Failure($"Participant {participantId} could not be found."));
                if (participant.TournamentId == tournamentId && participant.Matches.Any())
                    return BadRequest(Result.Failure("Participant with existing matches cannot be removed from a tournament"));
                var isCurrentUserParticipant = participant.Players.Any(p => p.Account.UserId == _currentUser.UserId);
                var isCurrentUserOrganiser = await IsCurrentUserOrganiser(tournamentId);
                if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser && !isCurrentUserParticipant)
                    return BadRequest(
                        Result.Failure("Only admins, tournament organiser and participant accounts are allowed to remove participant from existing competition!"));

                Logger.LogInformation($"{nameof(RemoveParticipantFromTournament)} requested for TournamentId: {tournamentId}, ParticipantId: {participantId}. RequestedBy -> Organiser: {isCurrentUserOrganiser}, Participant: {isCurrentUserParticipant}, Admin: {_currentUser.IsAdministrator}");
                var isSuccess = await _participants.DeletePermanently(participantId, _currentUser.UserId);
                return isSuccess ? Result.Success : Result.Failure($"Failed to remove participant {participantId} from tournament {tournamentId}");
            },
            msgOnError: $"Unexpected error during participant removal from tournament. ParticipantId: {participantId}. TournamentId: {tournamentId}");

    [HttpPost]
    [Route(nameof(GenerateTournamentDraw))]
    [Authorize]
    public async Task<ActionResult<Result>> GenerateTournamentDraw(int tournamentId)
        => await SafeHandle(async () =>
        {
            var isCurrentUserOrganiser = await IsCurrentUserOrganiser(tournamentId);
            if (!_currentUser.IsAdministrator && !isCurrentUserOrganiser)
                return Unauthorized(
                    Result.Failure(
                        "Only admins and tournament organiser are allowed to trigger tournament draw generation!"));
            
            var tournament = await _tournaments.GetForDrawGeneration(tournamentId);
            if (tournament == null)
                return BadRequest(Result.Failure($"Tournament {tournamentId} could not be found!"));

            if (tournament.Participants.Count() < tournament.MinParticipants)
                return BadRequest(Result.Failure(
                    $"Participants in tournament {tournamentId} are less than the configured minimal amount of participants!"));

            if (tournament.Matches.Any())
                return Result.Success;

            Logger.LogInformation($"Tournament draw generation for Tournament {tournamentId} has been triggered by user {_currentUser.UserId}");

            var isSuccess = await _tournamentDrawGenerator.GenerateTournamentDraw(tournament); 
            return isSuccess
                ? Result.Success
                : Result.Failure($"Failed to generate draw for tournament {tournamentId}");
        });

    private Seed ConvertParticipantToSeed(ParticipantShortOutputModel participant) 
        => new(participant.Id, participant.Name ?? string.Join($" /", participant.Players.Select(p => $"{p.FirstName} {p.LastName}")));


    private async Task<bool> IsCurrentUserOrganiser(int tournamentId) 
        => await _tournaments.GetOrganiserUserId(tournamentId) == _currentUser.UserId;

}
