namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Configurations;
using Contracts.Match;
using Contracts.Participant;
using Extensions;
using Interfaces.BL;
using Interfaces.Data;
using Microsoft.Extensions.Options;
using Models;
using Models.TournamentDrawGenerator;

public class TournamentDrawGenerator : ITournamentDrawGenerator
{
    private const short DefaultStartHour = 8;
    private const short DefaultEndHour = 8;
    
    private readonly IMatchesService _matchesService;
    private readonly IParticipantsService _participantsService;
    private readonly ITournamentsService _tournamentsService;
    private readonly IMatchesGenerator _matchesGenerator;
    private readonly IOptionsMonitor<TournamentCreationConfiguration> _tournamentConfiguration;
    private readonly ILogger<TournamentDrawGenerator> _logger;

    public TournamentDrawGenerator(
        IMatchesService matchesService,
        IParticipantsService participantsService,
        ITournamentsService tournamentsService,
        IMatchesGenerator matchesGenerator,
        IOptionsMonitor<TournamentCreationConfiguration> tournamentConfiguration,
        ILogger<TournamentDrawGenerator> logger)
    {
        _matchesService = matchesService;
        _participantsService = participantsService;
        _tournamentsService = tournamentsService;
        _matchesGenerator = matchesGenerator;
        _tournamentConfiguration = tournamentConfiguration;
        _logger = logger;
    }

    public async Task<bool> GenerateTournamentDraw(FullTournamentOutputModel tournament)
    {
        try
        {
            //await _matchesService.BeginTransaction();
            var dbTournament = await _tournamentsService.GetInternal(tournament.Id);
            var seeds = ConvertParticipantData(tournament.Participants, dbTournament.Type == TournamentType.Doubles).ToList();
            var matches = _matchesGenerator.GenerateMatches(seeds);
            var startHour = _tournamentConfiguration.CurrentValue.HasValidDailyStartHour()
                ? _tournamentConfiguration.CurrentValue.DailyStartHour
                : DefaultStartHour;;
            var endHour = _tournamentConfiguration.CurrentValue.HasValidDailyEndHour()
                ? _tournamentConfiguration.CurrentValue.DailyEndHour
                : DefaultEndHour;
            MatchScheduler.ScheduleMatches
                (matches, dbTournament.StartDate, dbTournament.EndDate, dbTournament.CourtsAvailable,
                    startTime: new TimeSpan(startHour, 0, 0), endTime: new TimeSpan(endHour, 0, 0));


            var drawMatchIdToDbMatchIdMap = new Dictionary<int, int>(matches.Count);
            foreach (var match in matches)
            {
                var dbMatchId = -1;
                if (match.HomeSeed == null || match.AwaySeed == null)
                {
                    dbMatchId = await _matchesService.CreateEmptyMatch(new MatchInputModel{Stage = match.TournamentStage, StartDate = match.StartTime, EndDate = match.EndTime}, dbTournament);
                    if (match is {HomeSeed: not null, AwaySeed: null})
                    {
                        var participant = await _participantsService.GetInternal(match.HomeSeed.Id);
                        await _matchesService.UpdateParticipant(dbMatchId, participant, isHome: true);
                    }
                    if (match is {HomeSeed: null, AwaySeed: not null})
                    {
                        var participant = await _participantsService.GetInternal(match.AwaySeed.Id);
                        await _matchesService.UpdateParticipant(dbMatchId, participant, isHome: false);
                    }
                }
                else
                {
                    var homeParticipant = await _participantsService.GetInternal(match.HomeSeed.Id);
                    var awayParticipant = await _participantsService.GetInternal(match.AwaySeed.Id);
                    dbMatchId = await _matchesService.Create(new MatchInputModel {Stage = match.TournamentStage, StartDate = match.StartTime, EndDate = match.EndTime},
                        dbTournament, homeParticipant, awayParticipant);
                }
                drawMatchIdToDbMatchIdMap.Add(match.Id, dbMatchId);
                if (match.HomePrevMatch.HasValue)
                    _ = await _matchesService.AddMatchFlow(dbTournament.Id, matchId: drawMatchIdToDbMatchIdMap[match.HomePrevMatch.Value], successorMatchId: dbMatchId, isWinnerHome: true);
                if (match.AwayPrevMatch.HasValue)
                    _ = await _matchesService.AddMatchFlow(dbTournament.Id, matchId: drawMatchIdToDbMatchIdMap[match.AwayPrevMatch.Value], successorMatchId: dbMatchId, isWinnerHome: false);
            }

            //await _matchesService.CommitTransaction();
            return true;
        }
        catch (Exception ex)
        {
            //await _matchesService.RollbackTransaction();
            //.LogError(ex, $"An error occured while drawing matches for tournament {tournament.Id}. Transactions were rollbacked");
            _logger.LogError(ex, $"An error occured while drawing matches for tournament {tournament.Id}.");
            return false;
        }
    }
    /// <summary>
    /// Orders participants based on Rating descending.
    /// Those with equal ratings are randomly shuffled in order to randomise the algorithm output and not always output the same draw in cases when exactly the same list of participants is included, which could be the case with guests.
    /// </summary>
    public static IEnumerable<Seed> ConvertParticipantData(IEnumerable<ParticipantShortOutputModel> participants, bool isDoublesTournament = false)
    {
        var rng = new Random();
        var orderedParticipants = participants
            .OrderByDescending(p => p.Players.Sum(pp => !isDoublesTournament ? pp.PlayerRating : pp.DoublesRating))
            .GroupBy(p => p.Players.Sum(pp => !isDoublesTournament ? pp.PlayerRating : pp.DoublesRating))
            .SelectMany(g => g.OrderBy(p => rng.Next()));

        return orderedParticipants.Select(p =>
            new Seed(p.Id, p.Name ?? string.Join("/", p.Players.Select(p => $"{p.FirstName} {p.LastName}"))));
    }
}