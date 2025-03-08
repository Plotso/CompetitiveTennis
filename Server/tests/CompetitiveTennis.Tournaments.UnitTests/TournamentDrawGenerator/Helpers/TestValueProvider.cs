namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator.Helpers;

public static class TestValueProvider
{
    public static IEnumerable<int> IntegersFrom1To256()
    {
        for (int i = 1; i <= 256; i++)
        {
            yield return i;
        }
    }
}