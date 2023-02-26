﻿namespace CompetitiveTennis.Tournaments.Data;

using CompetitiveTennis.Services.Interfaces;
using Models;
using Services.Interfaces;
using Tournaments.Models.Account;

public class TournamentDataSeeder : IDataSeeder
{
    private readonly IAccountsService _accounts;
    private readonly TournamentsDbContext _db;
    private readonly ILogger<TournamentDataSeeder> _logger;

    public TournamentDataSeeder(IAccountsService accounts, TournamentsDbContext db, ILogger<TournamentDataSeeder> logger)
    {
        _accounts = accounts;
        _db = db;
        _logger = logger;
    }

    public void SeedData()
    {
        Task.Run(async () =>
            {
                if (!_db.Accounts.Any(a => a.Username == Constants.SystemUser))
                {
                    var userId = Guid.NewGuid().ToString();
                    var accountInputModel = new AccountInputModel {FirstName = "System", LastName = "User"};
                    await _accounts.Create(new AccountCreateInputModel(userId, Constants.SystemUser, accountInputModel));
                    _logger.LogInformation("SysUser account seeded");
                }

                var sysAccount = await _accounts.GetSystemUser();
                
                //ToDo: Seed tournament
                //ToDo: Seed participants (sysUser & guest)
                //ToDo: Seed match (Use TournamentStage enum for stage)
                //ToDo: Seed scores 
            })
            .GetAwaiter()
            .GetResult();
    }
}