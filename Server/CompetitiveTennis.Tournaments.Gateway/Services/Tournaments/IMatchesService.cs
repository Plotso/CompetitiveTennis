namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Contracts;
using Contracts.Match;
using Contracts.MatchPeriod;
using Contracts.Tournament;
using Refit;

public interface IMatchesService
{
    [Get("/Matches/All")]
    Task<Result<IEnumerable<MatchOutputModel>>> All();

    [Get("/Matches/{id}")]
    Task<Result<MatchShortOutputModel>> ById(int id);

    [Get("/Matches/GetOrganiserUsername/{id}")]
    Task<Result<string>> GetOrganiserUsername(int id);

    [Post("/Matches/AddPeriodInfo/{id}")]
    Task<Result> AddPeriodInfo(int id, [Body] IEnumerable<MatchPeriodInputModel> matchPeriodInputs);

    [Get("/Matches/Search")]
    Task<Result<SearchOutputModel<MatchShortOutputModel>>> Search([Query] TournamentQuery query);
    
    [Post("/Matches/Add")]
    Task<Result<int>> Create(MatchInputModel input);
    
    [Put("/Matches/Edit/{id}")]
    Task<Result> Edit(int id, MatchInputModel input);

    [Delete("/Matches/{id}")]
    Task<Result> Delete(int id);
}