namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator;

using FluentAssertions;
using Helpers;
using Models.TournamentDrawGenerator;

[TestFixture]
public class SeedsSplitTests
{
    [Test, TestCaseSource(nameof(GetTestValues))]
    public void SplitSeeds_WithNPlayers_ShouldReturnCorrectSplit(int n)
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(n);
        var expectedLeft = players.Where((p, i) => IsLeft(i + 1)).ToList();
        var expectedRight = players.Where((p, i) => !IsLeft(i + 1)).ToList();

        // Act
        var split = MatchesGeneratorMoq.SplitSeedsMoq(players);

        // Assert
        split.LeftSplitSide.Should().Equal(expectedLeft, "left side should match expected players in order");
        split.RightSplitSide.Should().Equal(expectedRight, "right side should match expected players in order");
    }
    
    [Test]
    public void SplitSeeds_WithFortyPlayers_ShouldSplitCorrectly()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(40);

        // Act
        var result = MatchesGeneratorMoq.SplitSeedsMoq(players);

        // Expected pattern:
        // Cycle 1 (i=0-1):   i=0 Seed 1 (L), i=1 Seed 2 (R)
        // Cycle 2 (i=2-3):   i=2 Seed 3 (R), i=3 Seed 4 (L)
        // Cycle 3 (i=4-5):   i=4 Seed 5 (L), i=5 Seed 6 (R)
        // Cycle 4 (i=6-7):   i=6 Seed 7 (R), i=7 Seed 8 (L)
        // Cycle 5 (i=8-9):   i=8 Seed 9 (L), i=9 Seed 10 (R)
        // Cycle 6 (i=10-11): i=10 Seed 11 (R), i=11 Seed 12 (L)
        // Cycle 7 (i=12-13): i=12 Seed 13 (L), i=13 Seed 14 (R)
        // Cycle 8 (i=14-15): i=14 Seed 15 (R), i=15 Seed 16 (L)
        // Cycle 9 (i=16-17): i=16 Seed 17 (L), i=17 Seed 18 (R)
        // Cycle 10 (i=18-19): i=18 Seed 19 (R), i=19 Seed 20 (L)
        // Cycle 11 (i=20-21): i=20 Seed 21 (L), i=21 Seed 22 (R)
        // Cycle 12 (i=22-23): i=22 Seed 23 (R), i=23 Seed 24 (L)
        // Cycle 13 (i=24-25): i=24 Seed 25 (L), i=25 Seed 26 (R)
        // Cycle 14 (i=26-27): i=26 Seed 27 (R), i=27 Seed 28 (L)
        // Cycle 15 (i=28-29): i=28 Seed 29 (L), i=29 Seed 30 (R)
        // Cycle 16 (i=30-31): i=30 Seed 31 (R), i=31 Seed 32 (L)
        // Cycle 17 (i=32-33): i=32 Seed 33 (L), i=33 Seed 34 (R)
        // Cycle 18 (i=34-35): i=34 Seed 35 (R), i=35 Seed 36 (L)
        // Cycle 19 (i=36-37): i=36 Seed 37 (L), i=37 Seed 38 (R)
        // Cycle 20 (i=38-39): i=38 Seed 39 (R), i=39 Seed 40 (L)
        var expectedLeft = new[]
        {
            "Seed 1", "Seed 4", "Seed 5", "Seed 8", "Seed 9",
            "Seed 12", "Seed 13", "Seed 16", "Seed 17", "Seed 20",
            "Seed 21", "Seed 24", "Seed 25", "Seed 28", "Seed 29",
            "Seed 32", "Seed 33", "Seed 36", "Seed 37", "Seed 40"
        };
        
        var expectedRight = new[]
        {
            "Seed 2", "Seed 3", "Seed 6", "Seed 7", "Seed 10",
            "Seed 11", "Seed 14", "Seed 15", "Seed 18", "Seed 19",
            "Seed 22", "Seed 23", "Seed 26", "Seed 27", "Seed 30",
            "Seed 31", "Seed 34", "Seed 35", "Seed 38", "Seed 39"
        };

        // Assert
        result.LeftSplitSide.Select(p => p.Name).Should().Equal(expectedLeft);
        result.RightSplitSide.Select(p => p.Name).Should().Equal(expectedRight);
    }
    
    private static IEnumerable<int> GetTestValues()
    {
        for (int i = 0; i <= 256; i++)
        {
            yield return i;
        }
    }
    
    // Helper function to determine if a 1-based index belongs to the left side
    private static bool IsLeft(int k)
    {
        if (k == 1) return true;
        return ((k - 2) / 2) % 2 == 1;
    }

}