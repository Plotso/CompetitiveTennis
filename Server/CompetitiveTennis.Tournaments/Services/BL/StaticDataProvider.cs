namespace CompetitiveTennis.Tournaments.Services.BL;

using Models.TournamentDrawGenerator;

public static class StaticDataProvider
{
    public static readonly List<MatchGeneratorOutput> EmptyMatchGeneratorOutputsList = Enumerable.Empty<MatchGeneratorOutput>().ToList();
    public static readonly List<Seed> EmptySeedList = Enumerable.Empty<Seed>().ToList();
}