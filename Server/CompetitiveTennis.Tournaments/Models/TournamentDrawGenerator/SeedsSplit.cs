namespace CompetitiveTennis.Tournaments.Models.TournamentDrawGenerator;

using Services.BL;

public record SeedsSplit(List<Seed> LeftSplitSide, List<Seed> RightSplitSide)
{
    public static SeedsSplit Empty => new SeedsSplit(StaticDataProvider.EmptySeedList, StaticDataProvider.EmptySeedList);
}