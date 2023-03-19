namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Models;
using Models.Participant;
using Models.Tournament;
using Refit;

public interface ITournamentsService
{
    [Get("/Tournaments/All")]
    Task<Result<IEnumerable<TournamentOutputModel>>> All();

    [Get("/Tournaments/{id}")]
    Task<Result<TournamentOutputModel>> ById(int id);

    [Get("/Tournaments/Search")]
    Task<Result<SearchOutputModel<TournamentOutputModel>>> Search([Query] TournamentQuery query);
    
    [Post("/Tournaments/Add")]
    Task<Result<int>> Create(TournamentInputModel input);
    
    [Put("/Tournaments/Edit/{id}")]
    Task<Result> Edit(int id, TournamentInputModel input);
    
    [Put("/Tournaments/ChangeAvenue")]
    Task<Result> ChangeAvenue(int tournamentId, int newAvenueId);

    [Delete("/Tournaments/{id}")]
    Task<Result> Delete(int id);
    
    [Post("/Tournaments/AddGuest")]
    Task<Result<int>> AddGuest(ParticipantInputModel input);

    [Post("/Tournaments/ParticipateSingle")]
    Task<Result> ParticipateSingle(ParticipantInputModel input);

    [Post("/Tournaments/ParticipateDoubles")]
    Task<Result> ParticipateDoubles(MultiParticipantInputModel input);

    [Post("/Tournaments/AddAccountToParticipant")]
    Task<Result> AddAccountToParticipant(int tournamentId, int participantId, int accountId);

    [Post("/Tournaments/RemoveAccountFromParticipant")]
    Task<Result> RemoveAccountFromParticipant(int tournamentId, int participantId, int accountId);
    
    [Post("/Tournaments/RemoveParticipantFromTournament")]
    Task<Result> RemoveParticipantFromTournament(int tournamentId, int participantId);
}