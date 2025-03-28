﻿namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

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
    Task<Result> AddPeriodInfo(int id, [Body] MatchResultsInputModel matchResultsInputModel);

    [Post("/Matches/UpdateMatchOutcomeDueToCustomCondition/{id}")]
    Task<Result> UpdateMatchOutcomeDueToCustomCondition(int id,
        [Body] MatchCustomConditionResultInputModel matchCustomConditionResultInput);

    [Get("/Matches/Search")]
    Task<Result<SearchOutputModel<MatchShortOutputModel>>> Search([Query] MatchQuery query);
    
    [Post("/Matches/Add")]
    Task<Result<int>> Create(MatchInputModel input);
    
    [Put("/Matches/Edit/{id}")]
    Task<Result> Edit(int id, MatchInputModel input);

    [Delete("/Matches/DeleteMatchPeriods/{id}")]
    Task<Result> DeleteMatchPeriods(int id);

    [Delete("/Matches/DeleteMatchPeriodsAfterSetAndGameInclusive/{id}")]
    Task<Result> DeleteMatchPeriodsAfterSetAndGameInclusive(int id, int set, int game);

    [Delete("/Matches/{id}")]
    Task<Result> Delete(int id);
}