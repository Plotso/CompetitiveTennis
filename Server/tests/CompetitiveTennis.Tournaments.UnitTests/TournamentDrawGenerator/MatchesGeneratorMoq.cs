namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator;

using Models.TournamentDrawGenerator;
using Services.BL;

public class MatchesGeneratorMoq : MatchesGenerator
{
    public static int ClosestPowerOfTwoMoq(int n) => ClosestPowerOfTwo(n);
    
    public static int GetByesCountMoq(int seeds, int closesPowerOfTwo) => GetByesCount(seeds, closesPowerOfTwo);
    
    public static SeedsSplit SplitSeedsMoq(List<Seed> seeds) => SplitSeeds(seeds);
}