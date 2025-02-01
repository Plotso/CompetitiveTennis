namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Tournaments.Contracts.Account;
using CompetitiveTennis.Tournaments.Contracts.Match;
using CompetitiveTennis.Tournaments.Contracts.Tournament;
using CompetitiveTennis.Tournaments.Services.Interfaces.BL;
using CompetitiveTennis.Tournaments.Services.Interfaces.Data;

public class AccountStatsProvider : IAccountStatsProvider
{
    private readonly IMatchesService _matchesService;
    private readonly IAccountsService _accountsService;
    private readonly ITournamentsService _tournamentsService;

    public AccountStatsProvider(IMatchesService matchesService, IAccountsService accountsService, ITournamentsService tournamentsService)
    {
        _matchesService = matchesService;
        _accountsService = accountsService;
        _tournamentsService = tournamentsService;
    }
    
    public async ValueTask<AccountStats?> GetAccountStats(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        var totalTournamentsTask = await _tournamentsService.Total(new TournamentQuery(ParticipantUsernames: new[] { username }));
        var tournamentsWonTask = await _matchesService.Total(new MatchQuery(ParticipantUsername: username, Status: EventStatus.Ended, TournamentStage: TournamentStage.Final, IsParticipantWinner: true));
        var totalMatchesTask = await _matchesService.Total(new MatchQuery(ParticipantUsername: username));
        //var matchesWonTask = await _matchesService.Total(new MatchQuery(ParticipantUsername: username, IsParticipantWinner: true));
        var accountRatingInfoTask = await _accountsService.GetRatingForUsername(username);

        return new AccountStats(
            PlayerRating: accountRatingInfoTask.SinglesRating,
            DoublesRating: accountRatingInfoTask.DoublesRating,
            MatchesPlayed: totalMatchesTask,
            TournamentsPlayed: totalTournamentsTask,
            TournamentsWon: tournamentsWonTask
        );
    }
}