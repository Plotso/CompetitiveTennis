namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator.Helpers;

using Models.TournamentDrawGenerator;

public static class SeedTestDataProvider
{
    public static List<Seed> GetSeeds(int numberOfSeeds) => Enumerable.Range(1, numberOfSeeds)
        .Select(x => new Seed (Id: x, Name: $"Seed {x}"))
        .ToList();
}