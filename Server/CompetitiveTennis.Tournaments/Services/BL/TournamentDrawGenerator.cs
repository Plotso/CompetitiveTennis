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
    private readonly IOptionsMonitor<TournamentCreationConfiguration> _tournamentConfiguration;
    private readonly ILogger<TournamentDrawGenerator> _logger;

    public TournamentDrawGenerator(
        IMatchesService matchesService,
        IParticipantsService participantsService,
        ITournamentsService tournamentsService,
        IOptionsMonitor<TournamentCreationConfiguration> tournamentConfiguration,
        ILogger<TournamentDrawGenerator> logger)
    {
        _matchesService = matchesService;
        _participantsService = participantsService;
        _tournamentsService = tournamentsService;
        _tournamentConfiguration = tournamentConfiguration;
        _logger = logger;
    }

    public async Task<bool> GenerateTournamentDraw(FullTournamentOutputModel tournament)
    {
        try
        {
            //await _matchesService.BeginTransaction();
            var dbTournament = await _tournamentsService.GetInternal(tournament.Id);
            var seeds = ConvertParticipantData(tournament.Participants).ToList();
            var matches = GenerateMatches(seeds);
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
    
    public static IEnumerable<Seed> ConvertParticipantData(IEnumerable<ParticipantShortOutputModel> participants)
    {
        var rng = new Random();
        var orderedParticipants = participants
            .OrderByDescending(p => p.Players.Sum(p => p.PlayerRating))
            .GroupBy(p => p.Players.Sum(p => p.PlayerRating))
            .SelectMany(g => g.OrderBy(p => rng.Next()));

        return orderedParticipants.Select(p =>
            new Seed(p.Id, p.Name ?? string.Join("/", p.Players.Select(p => $"{p.FirstName} {p.LastName}"))));
    }
    public static List<MatchGeneratorOutput> GenerateMatches(List<Seed> seeds)
    { 
        var matches = new List<MatchGeneratorOutput>();
        if (seeds == null || !seeds.Any() || seeds.Count < 2)
            return matches;
        
        var numberOfPlayers = seeds.Count;
        var m = ClosestPowerOfTwo(numberOfPlayers);
        var byes = m - numberOfPlayers;
        
        var numRounds = (int)Math.Ceiling(Math.Log2(numberOfPlayers));
        var seedsSplit = SplitSeeds(seeds);
        var byesSplit = SplitByes(byes);
        var matchesGenerated = 1;


        if (seeds.Count == 2)
        {
            matches.Add(new MatchGeneratorOutput
            {
                Id = matchesGenerated++,
                PlayOrderNumber = matchesGenerated - 1,
                HomePlayer = seeds[0].Name,
                AwayPlayer = seeds[1].Name,
                HomeSeed = seeds[0],
                AwaySeed = seeds[1],
                TournamentStage = TournamentStageFromReversedRoundNumber(numRounds)
            });
            return matches;
        }

        var leftSplitQualifications = GenerateQualificationMatches(seedsSplit.LeftSplitSide, byesSplit.LeftSplitByes, ref matchesGenerated);
        var rightSplitQualifications = GenerateQualificationMatches(seedsSplit.RightSplitSide, byesSplit.RightSplitByes, ref matchesGenerated);
        matches.AddRange(leftSplitQualifications);
        matches.AddRange(rightSplitQualifications);
        var nonQualificationNumberOfRounds = byes == 0 ? numRounds : numRounds - 1;

        var leftSplitFirstRoundMatches = GenerateRoundMatchesWithSomeKnownSeeds(seedsSplit.LeftSplitSide, byesSplit.LeftSplitByes, leftSplitQualifications.Select(q => q.Id).ToList(), nonQualificationNumberOfRounds, ref matchesGenerated);
        var rightSplitFirstRoundMatches = GenerateRoundMatchesWithSomeKnownSeeds(seedsSplit.RightSplitSide, byesSplit.RightSplitByes, rightSplitQualifications.Select(q => q.Id).ToList(), nonQualificationNumberOfRounds, ref matchesGenerated);
        matches.AddRange(leftSplitFirstRoundMatches);
        matches.AddRange(rightSplitFirstRoundMatches);

        if (byes > 0)
            AdjustOrderOfFirstMainRoundMatches(matches.Where(m => m.TournamentStage == TournamentStageFromReversedRoundNumber(numRounds)));

        var prevRoundMatchIds = new List<int>();
        prevRoundMatchIds.AddRange(leftSplitFirstRoundMatches.Select(m => m.Id));
        prevRoundMatchIds.AddRange(rightSplitFirstRoundMatches.Select(m => m.Id));

        while (--nonQualificationNumberOfRounds >= 1)
        {
            var nextRoundMatches = GenerateRoundMatchesWithoutKnownSeeds(prevRoundMatchIds, nonQualificationNumberOfRounds, ref matchesGenerated);
            matches.AddRange(nextRoundMatches);
            prevRoundMatchIds = nextRoundMatches.Select(m => m.Id).ToList();
        }

        return matches;
    }

    private static void AdjustOrderOfFirstMainRoundMatches(IEnumerable<MatchGeneratorOutput> matches)
    {
        var matchesWithBothPlayers = matches.Where(m => m is {HomeSeed: not null, AwaySeed: not null});
        if (matchesWithBothPlayers.Count() == matches.Count())
            return;
        
        var remainingMatches = matches.Where(m => m.HomeSeed == null || m.AwaySeed == null);
        var firstMatchOrderNumber = matches.First().PlayOrderNumber;

        var orderNum = firstMatchOrderNumber;
        foreach (var match in matchesWithBothPlayers)
            match.PlayOrderNumber = orderNum++;
        foreach (var match in remainingMatches)
            match.PlayOrderNumber = orderNum++;

    }

    /// <summary>
    /// Generate round matches that have at least 1 know seed
    /// IMPORTANT - number of known seeds is equal to the number of byes
    /// </summary>
    private static List<MatchGeneratorOutput> GenerateRoundMatchesWithSomeKnownSeeds(List<Seed> seeds, int byes,
        List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated) =>
        GenerateRoundMatches(seeds, byes, prevRoundMatchIds, roundNum, ref matchesGenerated);

    /// <summary>
    /// Generating matches that would be played in later stage based on winner of previous matches
    /// </summary>
    private static List<MatchGeneratorOutput> GenerateRoundMatchesWithoutKnownSeeds(List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated) =>
        GenerateRoundMatches(seeds: null, byes: 0, prevRoundMatchIds, roundNum, ref matchesGenerated);

    private static List<MatchGeneratorOutput> GenerateRoundMatches(List<Seed>? seeds, int byes,
        List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated)
    {
        var matches = new List<MatchGeneratorOutput>();
        var seedsCount = seeds == null ? 0 : seeds.Count;

        var actualRoundSeeds = byes > 0 ? byes : seedsCount;
        var byeSeeds =   seeds != null ? seeds.Take(actualRoundSeeds).ToList() : seeds;
        var totalRoundPlayers = actualRoundSeeds + prevRoundMatchIds.Count;

        if (totalRoundPlayers == 0)
            totalRoundPlayers = prevRoundMatchIds.Count() / 2;

        var prevRoundMatchesUsed = 0;
        for (int i = 0; i < totalRoundPlayers / 2; i++)
        {
            var hasHomeSeed = byeSeeds != null && byeSeeds?.Count > i;
            var homePlayer = hasHomeSeed ? byeSeeds[i] : null;
            var homePlayerName = homePlayer?.Name ?? $"Winner of match {prevRoundMatchIds[prevRoundMatchesUsed++]}";
            var homePrevMatch = homePlayer == null ? prevRoundMatchIds[prevRoundMatchesUsed - 1] :(int?) null;
            
            var hasFillPlayer = prevRoundMatchesUsed < prevRoundMatchIds.Count;
            var awayPlayer = !hasFillPlayer ? byeSeeds[byeSeeds.Count - 1 - i + prevRoundMatchesUsed] : null;
            var awayPlayerName = awayPlayer?.Name ?? $"Winner of match {prevRoundMatchIds[prevRoundMatchesUsed++]}";
            var awayPrevMatch = awayPlayer == null ? prevRoundMatchIds[prevRoundMatchesUsed - 1] :(int?) null;
            
            var match = new MatchGeneratorOutput()
            {
                Id = matchesGenerated++,
                PlayOrderNumber = matchesGenerated - 1,
                HomePlayer = homePlayerName,
                AwayPlayer = awayPlayerName,
                HomeSeed = homePlayer,
                AwaySeed = awayPlayer,
                HomePrevMatch = homePrevMatch,
                AwayPrevMatch = awayPrevMatch,
                TournamentStage = TournamentStageFromReversedRoundNumber(roundNum)
            };
            matches.Add(match);
        }

        return matches;
    }

    private static List<MatchGeneratorOutput> GenerateQualificationMatches(List<Seed> seeds, int byes, ref int matchesGenerated)
    {
        var matches = new List<MatchGeneratorOutput>();
        if (byes == 0)
            return matches;
        
        var numberOfPlayers = seeds.Count;
        var qualificationPlayersCount = numberOfPlayers - byes;
        var qualificationPlayers = seeds.Skip(numberOfPlayers - qualificationPlayersCount).ToArray();
        if (qualificationPlayers.Any())
        {
            
            // The loop is reversed since players must be grouped from the middle 2 backwarrds
            for (int i = qualificationPlayers.Length / 2; i > 0 / 2; i--)
            {
                var match = new MatchGeneratorOutput()
                {
                    Id = matchesGenerated++,
                    PlayOrderNumber = matchesGenerated - 1,
                    HomePlayer = qualificationPlayers[i - 1].Name,
                    AwayPlayer = qualificationPlayers[qualificationPlayers.Length - i].Name,
                    HomeSeed = qualificationPlayers[i-1],
                    AwaySeed = qualificationPlayers[qualificationPlayers.Length - i],
                    TournamentStage = TournamentStage.Qualification
                };
                matches.Add(match);
            }
        }

        return matches;
    }
    

    private static TournamentStage TournamentStageFromReversedRoundNumber(int roundNumber) => roundNumber switch
    {
        0 => TournamentStage.Unknown,
        1 => TournamentStage.Final,
        2 => TournamentStage.SemiFinal,
        3 => TournamentStage.QuarterFinal,
        4 => TournamentStage.RoundOf16,
        5 => TournamentStage.RoundOf32,
        6 => TournamentStage.RoundOf64,
        7 => TournamentStage.RoundOf128,
        _ => TournamentStage.Qualification
    };
    
    
// Helper function to get the closest power of two greater than or equal to the given number
    private static int ClosestPowerOfTwo(int n)
    {
        int power = 0;
        while (Math.Pow(2, power) < n)
        {
            power++;
        }

        return (int) Math.Pow(2, power);
    }

    /// <summary>
    /// VERIFIED METHOD
    /// </summary>
    private static PlayerSplit SplitSeeds(List<Seed> players)
    {
        var splitSize = (int)Math.Ceiling((double) players.Count / 2);
        var leftPlayers = new List<Seed>(splitSize);
        var rightPlayers = new List<Seed>(splitSize);

        var sideCounter = 1;
        var isLeft = true;
        for (int i = 0; i < players.Count; i++)
        {
            sideCounter++;
            if (isLeft)
                leftPlayers.Add(players[i]);
            else
                rightPlayers.Add(players[i]);
            
            if (sideCounter >= 2)
            {
                isLeft = !isLeft;
                sideCounter = 0;
            }
        }

        return new PlayerSplit(leftPlayers, rightPlayers);
    }

    private static ByesSplit SplitByes(int totalByes)
    {
        if (totalByes == 0)
            return new ByesSplit(0, 0);
        var leftByes = 0;
        var rightByes = 0;
        
        var sideCounter = 1;
        var isLeft = true;
        for (int i = 0; i < totalByes; i++)
        {
            sideCounter++;
            if (isLeft)
                leftByes++;
            else
                rightByes++;
            
            if (sideCounter >= 2)
            {
                isLeft = !isLeft;
                sideCounter = 0;
            }
        }

        return new ByesSplit(leftByes, rightByes);
    }
}