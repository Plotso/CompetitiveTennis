namespace CompetitiveTennis.Tournaments.Services.BL;

using Models.TournamentDrawGenerator;
using Utils;

public static class MatchSeedPairProvider
{
    public static MatchSeedPair GetMatchPairForTwo(List<int> prevRoundMatchIds,
        ref int unassignedPreviousRoundMatchIndex, List<Seed>? byeSeeds)
    {
        var byeSeedCount = byeSeeds?.Count;
        var hasHomeSeed = byeSeeds != null && byeSeeds.Any();
        var homePlayer = hasHomeSeed ? byeSeeds[0] : null;
        var homePlayerName = homePlayer?.Name ?? MatchTemplateProviders.WinnerOfMatchSeedName(prevRoundMatchIds[unassignedPreviousRoundMatchIndex++]);
        var homePrevMatch = homePlayer == null ? prevRoundMatchIds[unassignedPreviousRoundMatchIndex - 1] :(int?) null;
        
        var hasAwaySeed = byeSeeds != null && byeSeeds.Any() && ((!hasHomeSeed && byeSeedCount == 1) || (hasHomeSeed && byeSeedCount == 2));
        var awayPlayer = hasAwaySeed ? byeSeeds[1] : null;
        var awayPlayerName = awayPlayer?.Name ?? MatchTemplateProviders.WinnerOfMatchSeedName(prevRoundMatchIds[unassignedPreviousRoundMatchIndex++]);
        var awayPrevMatch = awayPlayer == null ? prevRoundMatchIds[unassignedPreviousRoundMatchIndex - 1] :(int?) null;
        var matchPair = new MatchSeedPair()
        {
            HomePlayer = homePlayerName,
            AwayPlayer = awayPlayerName,
            HomeSeed = homePlayer,
            AwaySeed = awayPlayer,
            HomePrevMatch = homePrevMatch,
            AwayPrevMatch = awayPrevMatch,
        };
        return matchPair;
    }
    
    public static List<MatchSeedPair> GetMatchPairsWithoutSeeds(List<int> prevRoundMatchIds, int numberOfRoundMatches, int totalRoundPlayers, ref int unassignedPreviousRoundMatchIndex)
    {
        var matchPairs = new List<MatchSeedPair>(numberOfRoundMatches);
        for (int i = 0; i < totalRoundPlayers / 2; i++)
        {
            var homePlayerName = MatchTemplateProviders.WinnerOfMatchSeedName(prevRoundMatchIds[unassignedPreviousRoundMatchIndex++]);
            var homePrevMatch = prevRoundMatchIds[unassignedPreviousRoundMatchIndex - 1];
            
            
            var awayPlayerName = MatchTemplateProviders.WinnerOfMatchSeedName(prevRoundMatchIds[unassignedPreviousRoundMatchIndex++]);
            var awayPrevMatch = prevRoundMatchIds[unassignedPreviousRoundMatchIndex - 1];
            
            var matchPair = new MatchSeedPair()
            {
                HomePlayer = homePlayerName,
                AwayPlayer = awayPlayerName,
                HomeSeed = null,
                AwaySeed = null,
                HomePrevMatch = homePrevMatch,
                AwayPrevMatch = awayPrevMatch,
            };
            
            matchPairs.Add(matchPair);
        }

        return matchPairs;
    }
}