namespace CompetitiveTennis.Tournaments.Data;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Services.Interfaces;
using Models;
using Services.Interfaces;
using Tournaments.Models;
using Tournaments.Models.Account;
using Tournaments.Models.Avenue;
using Tournaments.Models.Enums;
using Tournaments.Models.Participant;
using Tournaments.Models.Tournament;

public class TournamentDataSeeder : IDataSeeder
{
    private readonly IAccountsService _accounts;
    private readonly IAvenuesService _avenues;
    private readonly ITournamentsService _tournaments;
    private readonly IParticipantsService _participants;
    private readonly TournamentsDbContext _db;
    private readonly ILogger<TournamentDataSeeder> _logger;

    public TournamentDataSeeder(
        IAccountsService accounts,
        IAvenuesService avenues,
        ITournamentsService tournaments,
        IParticipantsService participants,
        TournamentsDbContext db,
        ILogger<TournamentDataSeeder> logger)
    {
        _accounts = accounts;
        _avenues = avenues;
        _tournaments = tournaments;
        _participants = participants;
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
                        Country = "Bulgaria",
                        Courts = new List<CourtsInfo>
                        {
                            new() { Surface = Surface.Hard, AvailableCourtsByType = new Dictionary<CourtType, int>
                                {
                                    {CourtType.Outdoor, 5}
                                }},
                            new() { Surface = Surface.ArtificialGrass, AvailableCourtsByType = new Dictionary<CourtType, int>
                            {
                                {CourtType.Outdoor, 2}
                            }}
                        }
                    };
                    await _avenues.Create(avenue, sysAccount.UserId);
                    _logger.LogInformation("Barocco avenue seeded");
                }

                if (!_db.Tournaments.Any())
                {
                    var avenues = await _avenues.GetAll();
                    var avenueId = avenues.First().Id;
                    var avenue = await _avenues.GetInternal(avenueId);
                    
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
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(1).AddDays(1),
                        IsIndoor = false,
                        IsLeague = false,
                        AvenueId = avenueId
                    };
                    await _tournaments.Create(tournament, sysAccount, avenue);
                    _logger.LogInformation("BK-Test tournament seeded");
                }
                
                if (!_db.Participants.Any())
                {
                    var tournaments = await _tournaments.Query(new TournamentQuery(Keyword: "BK-Tournament"));
                    var dummyTournament = tournaments.FirstOrDefault();
                    if (dummyTournament != null)
                    {
                        var internalTournament = await _tournaments.GetInternal(dummyTournament.Id);
                        var guestParticipantInput = new ParticipantInputModel
                            {IsGuest = true, Name = "Guest", TournamentId = internalTournament.Id};
                        var sysUserParticipantInput = new ParticipantInputModel {TournamentId = internalTournament.Id};
                        _ = await _participants.Create(guestParticipantInput, internalTournament, team: null);
                        var sysUserParticipantId = await _participants.Create(sysUserParticipantInput, internalTournament, team: null);
                        await _participants.AddUsersToParticipant(sysUserParticipantId, new[] {sysAccount});
                    }
                }
                //ToDo: Seed match (Use TournamentStage enum for stage)
                //ToDo: Seed scores 
            })
            .GetAwaiter()
            .GetResult();
    }
}