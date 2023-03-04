namespace CompetitiveTennis.Tournaments.Data;

using CompetitiveTennis.Services.Interfaces;
using Models;
using Models.Enums;
using Services.Interfaces;
using Tournaments.Models.Account;
using Tournaments.Models.Avenue;
using Tournaments.Models.Tournament;

public class TournamentDataSeeder : IDataSeeder
{
    private readonly IAccountsService _accounts;
    private readonly IAvenuesService _avenues;
    private readonly ITournamentsService _tournaments;
    private readonly TournamentsDbContext _db;
    private readonly ILogger<TournamentDataSeeder> _logger;

    public TournamentDataSeeder(
        IAccountsService accounts,
        IAvenuesService avenues,
        ITournamentsService tournaments,
        TournamentsDbContext db,
        ILogger<TournamentDataSeeder> logger)
    {
        _accounts = accounts;
        _avenues = avenues;
        _tournaments = tournaments;
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

                if (!_db.Avenues.Any())
                {
                    var avenue = new AvenueInputModel
                    {
                        Name = "Barocco",
                        Location = "ул. 8-ми Декември №2",
                        City = "Sofia",
                        Country = "Bulgaria"
                    };
                    await _avenues.Create(avenue, sysAccount.UserId);
                    _logger.LogInformation("Barocco avenue seeded");
                }

                if (!_db.Tournaments.Any())
                {
                    var avenues = await _avenues.GetAll();
                    var avenue = avenues.First();
                    var tournament = new TournamentInputModel
                    {
                        //ToDo: Seed tournament
                        Title = "BK-Tournament",
                        Rules = "Best of 2 sets. Each set is won by the first player to win six games with a lead of at least two games. n all sets but the final set if the score reaches a 6 – 6 tie (rather than the highest possible 7 – 5 win) it triggers a tie-break. The final set is played until 2 games difference appears.",
                        Description = "Friendly tournament for non-professional players. If the weather is problematic, the start date may be changed",
                        Type = TournamentType.Singles,
                        Surface = Surface.Asphalt,
                        CourtsAvailable = 2,
                        MinParticipants = 4,
                        MaxParticipants = 32,
                        StartDate = new DateTime(2023, 4, 15, 10,0,0),
                        EndDate = new DateTime(2023, 4, 16, 12,0,0),
                        IsIndoor = false,
                        IsLeague = false,
                        AvenueId = avenue.Id
                    };
                    await _tournaments.Create(tournament, sysAccount);
                    _logger.LogInformation("BK-Test tournament seeded");
                }
                
                //ToDo: Seed participants (sysUser & guest)
                //ToDo: Seed match (Use TournamentStage enum for stage)
                //ToDo: Seed scores 
            })
            .GetAwaiter()
            .GetResult();
    }
}