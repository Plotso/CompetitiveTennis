namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Participant;
using Models.Tournament;
using Refit;

public interface ITournamentsService
{
    [Get("/Tournaments/All")]
    Task<ActionResult<IEnumerable<TournamentOutputModel>>> All();

    [Get("/Tournaments/{id}")]
    Task<ActionResult<TournamentOutputModel>> ById(int id);

    [Get("/Tournaments/Search")]
    Task<ActionResult<SearchOutputModel<TournamentOutputModel>>> Search([FromQuery] TournamentQuery query);
    
    [Post("/Tournaments/Add")]
    Task<ActionResult<int>> Create(TournamentInputModel input);
    
    [Put("/Tournaments/Edit/{id}")]
    Task<ActionResult> Edit(int id, TournamentInputModel input);
    
    [Put("/Tournaments/ChangeAvenue")]
    Task<ActionResult> ChangeAvenue(int tournamentId, int newAvenueId);

    [Delete("/Tournaments/{id}")]
    Task<ActionResult> Delete(int id);
    
    [Post("/Tournaments/AddGuest")]
    Task<ActionResult<int>> AddGuest(ParticipantInputModel input);

    [Post("/Tournaments/ParticipateSingle")]
    Task<ActionResult> ParticipateSingle(ParticipantInputModel input);

    [Post("/Tournaments/ParticipateDoubles")]
    Task<ActionResult> ParticipateDoubles(MultiParticipantInputModel input);

    [Post("/Tournaments/AddAccountToParticipant")]
    Task<ActionResult<bool>> AddAccountToParticipant(int tournamentId, int participantId, int accountId);

    [Post("/Tournaments/RemoveAccountFromParticipant")]
    Task<ActionResult<bool>> RemoveAccountFromParticipant(int tournamentId, int participantId, int accountId);
    
    [Post("/Tournaments/RemoveParticipantFromTournament")]
    Task<ActionResult<bool>> RemoveParticipantFromTournament(int tournamentId, int participantId);
}