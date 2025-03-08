namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Interfaces.BL;
using Models.TournamentDrawGenerator;
using Utils;

public class MatchesGenerator : IMatchesGenerator
{
    public List<MatchGeneratorOutput> GenerateMatches(List<Seed> seeds)
    {
        if (seeds == null || !seeds.Any() || seeds.Count < 2)
            return StaticDataProvider.EmptyMatchGeneratorOutputsList;
        
        
        var matches = new List<MatchGeneratorOutput>(seeds.Count - 1);
        
        var numberOfPlayers = seeds.Count;
        var m = ClosestPowerOfTwo(numberOfPlayers);
        var byes = GetByesCount(numberOfPlayers, m);
        
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
                TournamentStage = GetTournamentStageFromRoundNumber(numRounds)
            });
            return matches;
        }

        var leftSplitQualifications = GenerateQualificationMatches(seedsSplit.LeftSplitSide, byesSplit.LeftSplitByes, ref matchesGenerated);
        var rightSplitQualifications = GenerateQualificationMatches(seedsSplit.RightSplitSide, byesSplit.RightSplitByes, ref matchesGenerated);
        matches.AddRange(leftSplitQualifications);
        matches.AddRange(rightSplitQualifications);;
        var nonQualificationNumberOfRounds = byes == numberOfPlayers ? numRounds : numRounds - 1;
        if (seeds.Count == 3) // 3 seeds is an edge case since there will be no previous match ids passed to the recursive functions below. That's why it's handled explicitly here.
        {
            matches.Add(new MatchGeneratorOutput()
            {
                Id = matchesGenerated++,
                PlayOrderNumber = matchesGenerated - 1,
                HomePlayer = seeds[0].Name,
                AwayPlayer = MatchTemplateProviders.WinnerOfMatchSeedName(matches[0].Id),
                HomeSeed = seeds[0],
                AwaySeed = null,
                AwayPrevMatch = matches[0].Id,
                TournamentStage = GetTournamentStageFromRoundNumber(nonQualificationNumberOfRounds)
            });
            return matches;
        }


        var leftSplitFirstRoundMatches = GenerateRoundMatchesWithSomeKnownSeeds(seedsSplit.LeftSplitSide, byesSplit.LeftSplitByes, leftSplitQualifications.Select(q => q.Id).ToList(), nonQualificationNumberOfRounds, ref matchesGenerated);
        var rightSplitFirstRoundMatches = GenerateRoundMatchesWithSomeKnownSeeds(seedsSplit.RightSplitSide, byesSplit.RightSplitByes, rightSplitQualifications.Select(q => q.Id).ToList(), nonQualificationNumberOfRounds, ref matchesGenerated);
        matches.AddRange(leftSplitFirstRoundMatches);
        matches.AddRange(rightSplitFirstRoundMatches);

        if (byes > 0)
            AdjustOrderOfFirstMainRoundMatches(matches.Where(m => m.TournamentStage == GetTournamentStageFromRoundNumber(nonQualificationNumberOfRounds)));

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
        var matchesWithBothSeeds = matches.Where(m => m is {HomeSeed: not null, AwaySeed: not null});
        if (matchesWithBothSeeds.Count() == matches.Count())
            return;
        
        var remainingMatches = matches.Where(m => m.HomeSeed == null || m.AwaySeed == null);
        var firstMatchOrderNumber = matches.First().PlayOrderNumber;

        var orderNum = firstMatchOrderNumber;
        foreach (var match in matchesWithBothSeeds)
            match.PlayOrderNumber = orderNum++;
        foreach (var match in remainingMatches)
            match.PlayOrderNumber = orderNum++;

    }

    /// <summary>
    /// Generate round matches that have at least 1 know seed
    /// IMPORTANT - number of known seeds is equal to the number of byes
    /// </summary>
    private static List<MatchGeneratorOutput> GenerateRoundMatchesWithSomeKnownSeeds(List<Seed> seeds, int byes,
        List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated, int unassignedPreviousRoundMatchIndex = 0) 
        => GenerateRoundMatches(seeds, byes, prevRoundMatchIds, roundNum, ref matchesGenerated, ref unassignedPreviousRoundMatchIndex);

    /// <summary>
    /// Generating matches that would be played in later stage based on winner of previous matches
    /// </summary>
    private static List<MatchGeneratorOutput> GenerateRoundMatchesWithoutKnownSeeds(List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated, int unassignedPreviousRoundMatchIndex = 0) 
        => GenerateRoundMatchesWithoutSeedsAndByes(prevRoundMatchIds, roundNum, totalRoundPlayers: prevRoundMatchIds.Count, ref matchesGenerated, ref unassignedPreviousRoundMatchIndex);

    private static List<MatchGeneratorOutput> GenerateRoundMatches(List<Seed>? seeds, int byes,
        List<int> prevRoundMatchIds, int roundNum, ref int matchesGenerated, ref int unassignedPreviousRoundMatchIndex)
    {
        var seedsCount = seeds?.Count ?? 0;
        var byesCount = byes;
        var byeSeeds = seeds?.Take(byesCount).ToList();
        
        var unassignedPrevRoundMatchIds = prevRoundMatchIds.Count - unassignedPreviousRoundMatchIndex;
        var totalRoundPlayers = byesCount + unassignedPrevRoundMatchIds;

        if (totalRoundPlayers == 0)
            return StaticDataProvider.EmptyMatchGeneratorOutputsList;
        
        if (byesCount == 0) 
            return GenerateRoundMatchesWithoutSeedsAndByes(prevRoundMatchIds, roundNum, totalRoundPlayers: seedsCount / 2, ref matchesGenerated, ref unassignedPreviousRoundMatchIndex);

        var isByeAndQualifiers = byesCount == 1 && seedsCount == 3 && seeds?.Count(s => s.IsQualificationPlayer) == 2;
        if (seedsCount <= 2 || isByeAndQualifiers)// || (actualRoundSeeds <= 2 && unassignedPrevRoundMatchIds >= 0))
        {
            var matchPair = MatchSeedPairProvider.GetMatchPairForTwo(prevRoundMatchIds, ref unassignedPreviousRoundMatchIndex, byeSeeds);
            return new List<MatchGeneratorOutput>(1)
            {
                new MatchGeneratorOutput()
                {
                    Id = matchesGenerated++,
                    PlayOrderNumber = matchesGenerated - 1,
                    HomePlayer = matchPair.HomePlayer,
                    AwayPlayer = matchPair.AwayPlayer,
                    HomeSeed = matchPair.HomeSeed,
                    AwaySeed = matchPair.AwaySeed,
                    HomePrevMatch = matchPair.HomePrevMatch,
                    AwayPrevMatch = matchPair.AwayPrevMatch,
                    TournamentStage = GetTournamentStageFromRoundNumber(roundNum)
                }
            };
        }
        
        var split = seeds != null && seeds.Any() ? SplitSeeds(seeds) : SeedsSplit.Empty;
        
        var numberOfRoundMatches = totalRoundPlayers / 2;
        var matches = new List<MatchGeneratorOutput>(numberOfRoundMatches);
        matches.AddRange(GenerateRoundMatches(split.LeftSplitSide, split.LeftSplitSide.Count(s => !s.IsQualificationPlayer), prevRoundMatchIds, roundNum, ref matchesGenerated, ref unassignedPreviousRoundMatchIndex));
        matches.AddRange(GenerateRoundMatches(split.RightSplitSide, split.RightSplitSide.Count(s => !s.IsQualificationPlayer), prevRoundMatchIds, roundNum, ref matchesGenerated, ref unassignedPreviousRoundMatchIndex));
        return matches;
    }

    private static List<MatchGeneratorOutput> GenerateQualificationMatches(List<Seed> seeds, int byes, ref int matchesGenerated)
    {
        
        if (seeds.Count == 0)
            return StaticDataProvider.EmptyMatchGeneratorOutputsList;
        
        var numberOfPlayers = seeds.Count;
        var qualificationPlayersCount = numberOfPlayers - byes;
        var qualificationPlayers = seeds.Skip(numberOfPlayers - qualificationPlayersCount).ToList();
        qualificationPlayers.ForEach(s => s.IsQualificationPlayer = true);
        return ProcessQualificationPlayersRecursively(seeds, ref matchesGenerated);

    }
    
    private static List<MatchGeneratorOutput> ProcessQualificationPlayersRecursively(List<Seed> seeds, ref int matchesGenerated)
    {
        if (seeds == null || !seeds.Any() || seeds.Count < 2)
            return StaticDataProvider.EmptyMatchGeneratorOutputsList;
        if (seeds.Count == 2 && seeds.All(s => s.IsQualificationPlayer))
            return new List<MatchGeneratorOutput>
            {
                new MatchGeneratorOutput
                {
                    Id = matchesGenerated++,
                    PlayOrderNumber = matchesGenerated - 1,
                    HomePlayer = seeds[0].Name,
                    AwayPlayer = seeds[1].Name,
                    HomeSeed = seeds[0],
                    AwaySeed = seeds[1],
                    TournamentStage = TournamentStage.Qualification

                }
            };
        if (seeds.Count == 3 && seeds.Skip(1).All(s => s.IsQualificationPlayer))
        {
            return new List<MatchGeneratorOutput>
            {
                new MatchGeneratorOutput
                {
                    Id = matchesGenerated++,
                    PlayOrderNumber = matchesGenerated - 1,
                    HomePlayer = seeds[1].Name,
                    AwayPlayer = seeds[2].Name,
                    HomeSeed = seeds[1],
                    AwaySeed = seeds[2],
                    TournamentStage = TournamentStage.Qualification

                }
            };
        }
        
        var seedsSplit = SplitSeeds(seeds);
        var leftSplitMatches = ProcessQualificationPlayersRecursively(seedsSplit.LeftSplitSide, ref matchesGenerated);
        var rightSplitMatches = ProcessQualificationPlayersRecursively(seedsSplit.RightSplitSide, ref matchesGenerated);
        return leftSplitMatches.Concat(rightSplitMatches).ToList();
    }
    

    private static TournamentStage GetTournamentStageFromRoundNumber(int roundNumber) => roundNumber switch
    {
        0 => TournamentStage.Unknown,
        1 => TournamentStage.Final,
        2 => TournamentStage.SemiFinal,
        3 => TournamentStage.QuarterFinal,
        4 => TournamentStage.RoundOf16,
        5 => TournamentStage.RoundOf32,
        6 => TournamentStage.RoundOf64,
        7 => TournamentStage.RoundOf128,
        8 => TournamentStage.RoundOf256,
        _ => TournamentStage.Qualification
    };
    
    
    /// <summary>
    /// Helper function to get the closest power of two lower than or equal to the given number
    /// </summary>
    protected static int ClosestPowerOfTwo(int n)
    {
        if (n <= 0)
            throw new ArgumentException("Number of players must be positive.");
        
        var highestBit = 0;
        while ((1 << (highestBit + 1)) <= n)
        {
            highestBit++;
        }
        return 1 << highestBit;
    }
    
    
    
    /// <param name="s">Seeds count</param>
    /// <param name="m">Closes power of two that's lower than or equal to seeds count</param>
    /// <returns></returns>
    protected static int GetByesCount(int s, int m)
    {
        if (s == m) return m;

        return (m * 2) - s;
    }

    protected static SeedsSplit SplitSeeds(List<Seed> players)
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

        return new SeedsSplit(leftPlayers, rightPlayers);
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

            if (sideCounter < 2) continue;
            isLeft = !isLeft;
            sideCounter = 0;
        }

        return new ByesSplit(leftByes, rightByes);
    }
    
    private static List<MatchGeneratorOutput> GenerateRoundMatchesWithoutSeedsAndByes(
        List<int> prevRoundMatchIds, int roundNum, int totalRoundPlayers, ref int matchesGenerated, ref int unassignedPreviousRoundMatchIndex)
    {
        var numberOfRoundMatches = totalRoundPlayers / 2;
        var matches = new List<MatchGeneratorOutput>(numberOfRoundMatches);

        var tournamentStage = GetTournamentStageFromRoundNumber(roundNum);
        var matchPairs = MatchSeedPairProvider.GetMatchPairsWithoutSeeds(prevRoundMatchIds, numberOfRoundMatches, totalRoundPlayers, ref unassignedPreviousRoundMatchIndex);
        for (int i = 0; i < numberOfRoundMatches; i++)
        {
            var matchPair = matchPairs[i];
            var match = new MatchGeneratorOutput()
            {
                Id = matchesGenerated++,
                PlayOrderNumber = matchesGenerated - 1,
                HomePlayer = matchPair.HomePlayer,
                AwayPlayer = matchPair.AwayPlayer,
                HomeSeed = matchPair.HomeSeed,
                AwaySeed = matchPair.AwaySeed,
                HomePrevMatch = matchPair.HomePrevMatch,
                AwayPrevMatch = matchPair.AwayPrevMatch,
                TournamentStage = tournamentStage
            };
            matches.Add(match);
        }
        return matches;
    }
}