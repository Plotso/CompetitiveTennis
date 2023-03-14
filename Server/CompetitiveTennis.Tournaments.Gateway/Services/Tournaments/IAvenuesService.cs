namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Avenue;
using Refit;

public interface IAvenuesService
{
    [Get("/Avenues/All")]
    Task<ActionResult<IEnumerable<AvenueOutputModel>>> All();

    [Get("/Avenues/{id}")]
    Task<ActionResult<AvenueOutputModel>> ById(int id);

    [Get("/Avenues/Search")]
    Task<ActionResult<SearchOutputModel<AvenueOutputModel>>> Search([Query] AvenueQuery query);
    
    [Post("/Avenues")]
    Task<ActionResult<Result<int>>> Create(AvenueInputModel input);
    
    [Put("/Avenues/{id}")]
    Task<ActionResult> Edit(int id, AvenueInputModel input);

    [Delete("/Avenues/{id}")]
    Task<ActionResult> Delete(int id);
}