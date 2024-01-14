namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Contracts;
using Contracts.Avenue;
using Refit;

public interface IAvenuesService
{
    [Get("/Avenues/All")]
    Task<Result<IEnumerable<AvenueOutputModel>>> All();

    [Get("/Avenues/{id}")]
    Task<Result<AvenueOutputModel>> ById(int id);

    [Get("/Avenues/Search")]
    Task<Result<SearchOutputModel<AvenueOutputModel>>> Search([Query] AvenueQuery query);
    
    [Post("/Avenues")]
    Task<Result<int>> Create(AvenueInputModel input);
    
    [Put("/Avenues/{id}")]
    Task<Result> Edit(int id, AvenueInputModel input);

    [Delete("/Avenues/{id}")]
    Task<Result> Delete(int id);
}