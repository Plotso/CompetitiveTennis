﻿namespace CompetitiveTennis.Tournaments.Gateway.Services.Tournaments;

using CompetitiveTennis.Models;
using Models.Account;
using Refit;

public interface IAccountsService
{
    [Get("/Accounts/All")]
    Task<Result<IEnumerable<AccountOutputModel>>> All();
    
    [Get("/Accounts/{id}")]
    Task<Result<AccountOutputModel>> ById(int id);

    [Post("/Accounts")]
    Task<Result> Add(AccountInputModel input);
    
    [Put("/Accounts/ChangeNames/{id}")]
    Task<Result> ChangeNames(int id, AccountInputModel input);
    
}