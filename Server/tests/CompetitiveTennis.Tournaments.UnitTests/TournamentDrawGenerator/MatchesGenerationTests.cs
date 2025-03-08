namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator;

using CompetitiveTennis.Data.Models.Enums;
using FluentAssertions;
using Helpers;
using Models.TournamentDrawGenerator;
using Services.BL;
using Services.Interfaces.BL;

[TestFixture]
public class MatchesGenerationTests
{
    private IMatchesGenerator _matchesGenerator;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _matchesGenerator = new MatchesGenerator();
    }
    
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(11)]
    [TestCase(12)]
    [TestCase(13)]
    [TestCase(31)]
    [TestCase(32)]
    [TestCase(36)]
    [TestCase(40)]
    [TestCase(41)]
    [TestCase(42)]
    [TestCase(84)]
    [TestCase(86)]
    [TestCase(100)]
    public void GenerateMatchesNew_WithNSeeds_ShouldGenerateCorrectOutput(int numberOfSeeds)
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(numberOfSeeds);
        var matches = _matchesGenerator.GenerateMatches(players);
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var expectedMatch =
                ExpectedMatchTestDataProvider.GetExpectedMatchBasedOnMatchNumberAndNumberOfSeeds(matchNumber: i + 1,
                    numberOfSeeds);
            match.PlayOrderNumber.Should().Be(expectedMatch.PlayOrderNumber);
            match.TournamentStage.Should().Be(expectedMatch.TournamentStage);
            match.HomeSeedPlaceholder.Should().Be(expectedMatch.HomeSeedPlaceholder);
            match.AwaySeedPlaceholder.Should().Be(expectedMatch.AwaySeedPlaceholder);
            match.HomePrevMatch.Should().Be(expectedMatch.HomePrevMatch);
            match.AwayPrevMatch.Should().Be(expectedMatch.AwayPrevMatch);
        }
    }
    
    
    [Test, TestCaseSource(nameof(GetTestValues))]
    public void GenerateMatchesNew_WithNSeeds_ShouldOutputMatchesCount(int n)
    {
        var players = SeedTestDataProvider.GetSeeds(n);
        var matches = _matchesGenerator.GenerateMatches(players);
        var closestPowerOfTwo = MatchesGeneratorMoq.ClosestPowerOfTwoMoq(n);
        var hasByes = n != closestPowerOfTwo;
        var byes = MatchesGeneratorMoq.GetByesCountMoq(n, closestPowerOfTwo);
        var qualificantsCount = n - byes;

        matches.Should().HaveCount(n - 1);
        matches.Any(m => m.TournamentStage == TournamentStage.Qualification).Should().Be(hasByes);
        matches.Where(m => m.TournamentStage == TournamentStage.Qualification).Should().HaveCount(qualificantsCount / 2);
        var qualificationMatches = matches.Where(m => m.TournamentStage == TournamentStage.Qualification).ToList();
        var nonQualificationMatches = matches.Where(m => m.TournamentStage != TournamentStage.Qualification).ToList();
        var qualificants = players.Skip(byes);
        foreach (var qualificant in qualificants)
        {
            //Ensure qualificant is present in qualifications matches
            qualificationMatches.Any(m => m.HomeSeedPlaceholder == qualificant.Name || m.AwaySeedPlaceholder == qualificant.Name).Should().BeTrue();
            //Ensure qualificant ain't present in any main draw match
            nonQualificationMatches.All(m => m.HomeSeedPlaceholder != qualificant.Name && m.AwaySeedPlaceholder != qualificant.Name).Should().BeTrue();
        }
        
        var byePlayers = players.Take(byes);
        foreach (var byePlayer in byePlayers)
        {
            // Ensure no bye is present in qualification match
            qualificationMatches.All(m => m.HomeSeedPlaceholder != byePlayer.Name && m.AwaySeedPlaceholder != byePlayer.Name).Should().BeTrue();
            if (n > 1)
            {
                // Ensure bye is present in main draw match
                nonQualificationMatches.Any(m => m.HomeSeedPlaceholder == byePlayer.Name || m.AwaySeedPlaceholder == byePlayer.Name).Should().BeTrue();
            }
        }
    }

    [Test]
    public void GenerateMatchesNew_With10Seeds_ShouldHaveCorrectQualificationOrder()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(10);
        var matches = _matchesGenerator.GenerateMatches(players);
        var qualificationMatches = matches.Where(m => m.TournamentStage == TournamentStage.Qualification)
            .OrderBy(m => m.PlayOrderNumber)
            .ToList();

        // Act & Assert
        qualificationMatches.Should().HaveCount(2, "10 seeds should produce 2 qualification matches");
        qualificationMatches[0].PlayOrderNumber.Should()
            .Be(1, "First qualification match should have PlayOrderNumber 1");
        qualificationMatches[1].PlayOrderNumber.Should()
            .Be(2, "Second qualification match should have PlayOrderNumber 2");
    }
    
    [Test]
    public void GenerateMatchesNew_With64Seeds_ShouldGenerateCorrectMainDraw()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(64);
        var matches = _matchesGenerator.GenerateMatches(players);
        var firstRoundMatches = matches.Where(m => m.TournamentStage == TournamentStage.RoundOf64)
            .OrderBy(m => m.PlayOrderNumber)
            .ToList();

        // Act
        var firstMatch = firstRoundMatches[0]; // Match 1
        var secondMatch = firstRoundMatches[1]; // Match 2
        var thirdMatch = firstRoundMatches[2]; // Match 3
        var lastMatch = firstRoundMatches[31]; // Match 32

        // Assert
        firstRoundMatches.Should().HaveCount(32, "64 seeds should produce 32 first-round matches");

        firstMatch.Id.Should().Be(1, "First match should be M1");
        firstMatch.PlayOrderNumber.Should().Be(1);
        firstMatch.HomeSeedPlaceholder.Should().Be("Seed 1");
        firstMatch.AwaySeedPlaceholder.Should().Be("Seed 64");

        secondMatch.Id.Should().Be(2, "Second match should be M2");
        secondMatch.PlayOrderNumber.Should().Be(2);
        secondMatch.HomeSeedPlaceholder.Should().Be("Seed 32");
        secondMatch.AwaySeedPlaceholder.Should().Be("Seed 33");

        thirdMatch.Id.Should().Be(3, "Third match should be M3");
        thirdMatch.PlayOrderNumber.Should().Be(3);
        thirdMatch.HomeSeedPlaceholder.Should().Be("Seed 16");
        thirdMatch.AwaySeedPlaceholder.Should().Be("Seed 49");

        lastMatch.Id.Should().Be(32, "Last match should be M32");
        lastMatch.PlayOrderNumber.Should().Be(32);
        lastMatch.HomeSeedPlaceholder.Should().Be("Seed 22");
        lastMatch.AwaySeedPlaceholder.Should().Be("Seed 43");
    }

    [Test]
    public void GenerateMatchesNew_With64Seeds_ShouldGenerateAllFirstRoundMatchesCorrectly()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(64);
        var matches = _matchesGenerator.GenerateMatches(players);
        var firstRoundMatches = matches.Where(m => m.TournamentStage == TournamentStage.RoundOf64)
            .OrderBy(m => m.PlayOrderNumber)
            .ToList();

        // Expected pairings based on your output
        var expectedPairings = new List<(string Home, string Away)>
        {
            ("Seed 1", "Seed 64"), ("Seed 32", "Seed 33"), ("Seed 16", "Seed 49"), ("Seed 17", "Seed 48"),
            ("Seed 8", "Seed 57"), ("Seed 25", "Seed 40"), ("Seed 9", "Seed 56"), ("Seed 24", "Seed 41"),
            ("Seed 4", "Seed 61"), ("Seed 29", "Seed 36"), ("Seed 13", "Seed 52"), ("Seed 20", "Seed 45"),
            ("Seed 5", "Seed 60"), ("Seed 28", "Seed 37"), ("Seed 12", "Seed 53"), ("Seed 21", "Seed 44"),
            ("Seed 2", "Seed 63"), ("Seed 31", "Seed 34"), ("Seed 15", "Seed 50"), ("Seed 18", "Seed 47"),
            ("Seed 7", "Seed 58"), ("Seed 26", "Seed 39"), ("Seed 10", "Seed 55"), ("Seed 23", "Seed 42"),
            ("Seed 3", "Seed 62"), ("Seed 30", "Seed 35"), ("Seed 14", "Seed 51"), ("Seed 19", "Seed 46"),
            ("Seed 6", "Seed 59"), ("Seed 27", "Seed 38"), ("Seed 11", "Seed 54"), ("Seed 22", "Seed 43")
        };

        // Assert
        firstRoundMatches.Should().HaveCount(32, "64 seeds should produce 32 first-round matches");

        for (int i = 0; i < 32; i++)
        {
            var match = firstRoundMatches[i];
            match.Id.Should().Be(i + 1, $"Match {i + 1} should have correct ID");
            match.PlayOrderNumber.Should().Be(i + 1);
            match.HomeSeedPlaceholder.Should().Be(expectedPairings[i].Home, $"Match {i + 1} home player mismatch");
            match.AwaySeedPlaceholder.Should().Be(expectedPairings[i].Away, $"Match {i + 1} away player mismatch");
        }
    }

    // Test 4: Followup Rounds are Correctly Generated
    [Test]
    public void GenerateMatchesNew_With64Seeds_ShouldGenerateCorrectFollowupRounds()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(64);
        var matches = _matchesGenerator.GenerateMatches(players);
        var roundOf32Matches = matches.Where(m => m.TournamentStage == TournamentStage.RoundOf32)
            .OrderBy(m => m.PlayOrderNumber)
            .ToList();
        var roundOf16Matches = matches.Where(m => m.TournamentStage == TournamentStage.RoundOf16)
            .OrderBy(m => m.PlayOrderNumber)
            .ToList();

        // Act
        var firstR32Match = roundOf32Matches[0];
        var secondR32Match = roundOf32Matches[1];
        var firstR16Match = roundOf16Matches[0];

        // Assert
        roundOf32Matches.Should().HaveCount(16, "Round of 32 should have 16 matches");
        firstR32Match.Id.Should().Be(33, "First Round of 32 match should be M33");
        firstR32Match.PlayOrderNumber.Should().Be(33, "PlayOrderNumber should match Id");
        firstR32Match.HomeSeedPlaceholder.Should().Be("Winner of match 1", "M33 should pair W1 vs W2");
        firstR32Match.AwaySeedPlaceholder.Should().Be("Winner of match 2");
        secondR32Match.Id.Should().Be(34, "Second Round of 32 match should be M34");
        secondR32Match.HomeSeedPlaceholder.Should().Be("Winner of match 3", "M34 should pair W3 vs W4");
        secondR32Match.AwaySeedPlaceholder.Should().Be("Winner of match 4");

        roundOf16Matches.Should().HaveCount(8, "Round of 16 should have 8 matches");
        firstR16Match.Id.Should().Be(49, "First Round of 16 match should be M49");
        firstR16Match.PlayOrderNumber.Should().Be(49, "PlayOrderNumber should match Id");
        firstR16Match.HomeSeedPlaceholder.Should().Be("Winner of match 33", "M49 should pair W33 vs W34");
        firstR16Match.AwaySeedPlaceholder.Should().Be("Winner of match 34");
    }

    // Edge Case: 3 Seeds (Minimal Qualification + Main Draw)
    [Test]
    public void GenerateMatchesNew_With3Seeds_ShouldGenerateCorrectEdgeCase()
    {
        // Arrange
        var players = SeedTestDataProvider.GetSeeds(3); // m=2, byes=1, QP=2 → 1 qual match
        var matches = _matchesGenerator.GenerateMatches(players);
        var qualMatch = matches.Where(m => m.TournamentStage == TournamentStage.Qualification)
            .Single();
        var mainMatch = matches.Where(m => m.TournamentStage == TournamentStage.Final)
            .Single();

        // Assert
        matches.Should().HaveCount(2, "3 seeds should produce 2 total matches");
        qualMatch.Id.Should().Be(1, "Qualification match should be M1");
        qualMatch.PlayOrderNumber.Should().Be(1);
        qualMatch.HomeSeedPlaceholder.Should().Be("Seed 2", "Qualification should pair Seed 2 vs Seed 3");
        qualMatch.AwaySeedPlaceholder.Should().Be("Seed 3");

        mainMatch.Id.Should().Be(2, "Main match should be M2");
        mainMatch.PlayOrderNumber.Should().Be(2);
        mainMatch.HomeSeedPlaceholder.Should().Be("Seed 1", "Seed 1 should face qualifier");
        mainMatch.AwaySeedPlaceholder.Should().Be("Winner of match 1");
    }
    
    [Test]
    public void GenerateMatchesNew_With2Seeds_ShouldGenerateCorrectTournament()
    {
        var players = SeedTestDataProvider.GetSeeds(2);
        var matches = _matchesGenerator.GenerateMatches(players);
        ValidateTournamentStructure(matches, 2);
    }

    private void ValidateTournamentStructure(List<MatchGeneratorOutput> matches, int seedCount)
    {
        var expectedPairings = GetExpectedFirstRoundPairings(seedCount);
        var firstRoundStage = GetFirstRoundStage(seedCount);
        var stages = new List<TournamentStage>
        {
            TournamentStage.RoundOf128, TournamentStage.RoundOf64, TournamentStage.RoundOf32,
            TournamentStage.RoundOf16, TournamentStage.QuarterFinal, TournamentStage.SemiFinal,
            TournamentStage.Final
        }.SkipWhile(s => s != firstRoundStage).ToList();

        int matchIndex = 0;
        int matchCount = seedCount / 2;

        foreach (var stage in stages)
        {
            var roundMatches = matches.Skip(matchIndex).Take(matchCount)
                .OrderBy(m => m.PlayOrderNumber)
                .ToList();

            for (int i = 0; i < roundMatches.Count; i++)
            {
                var match = roundMatches[i];
                int expectedId = matchIndex + i + 1;

                // Common assertions
                match.Id.Should().Be(expectedId, $"Match {expectedId} should have correct ID");
                match.PlayOrderNumber.Should().Be(expectedId, $"Match {expectedId} should have correct play order");
                match.TournamentStage.Should().Be(stage, $"Match {expectedId} should be in {stage}");

                if (stage == firstRoundStage)
                {
                    // First round: validate seed pairings
                    match.HomeSeedPlaceholder.Should().Be(expectedPairings[i].Home, $"Match {expectedId} home player mismatch");
                    match.AwaySeedPlaceholder.Should().Be(expectedPairings[i].Away, $"Match {expectedId} away player mismatch");
                    match.HomePrevMatch.Should().BeNull($"Match {expectedId} should have no home previous match");
                    match.AwayPrevMatch.Should().BeNull($"Match {expectedId} should have no away previous match");
                }
                else
                {
                    // Subsequent rounds: validate winner progression
                    int homePrevId = matchIndex - matchCount + i * 2 + 1;
                    int awayPrevId = homePrevId + 1;
                    match.HomeSeedPlaceholder.Should().Be($"Winner of match {homePrevId}",
                        $"Match {expectedId} home player mismatch");
                    match.AwaySeedPlaceholder.Should().Be($"Winner of match {awayPrevId}",
                        $"Match {expectedId} away player mismatch");
                    match.HomePrevMatch.Should().Be(homePrevId,
                        $"Match {expectedId} should reference home prev match {homePrevId}");
                    match.AwayPrevMatch.Should().Be(awayPrevId,
                        $"Match {expectedId} should reference away prev match {awayPrevId}");
                }
            }

            matchIndex += matchCount;
            matchCount /= 2;
            if (matchCount == 0) break;
        }

        matches.Count.Should().Be(matchIndex, "Total number of matches should match expected tournament size");
    }

    private TournamentStage GetFirstRoundStage(int seedCount)
    {
        return seedCount switch
        {
            2 => TournamentStage.Final,
            4 => TournamentStage.SemiFinal,
            8 => TournamentStage.QuarterFinal,
            16 => TournamentStage.RoundOf16,
            32 => TournamentStage.RoundOf32,
            64 => TournamentStage.RoundOf64,
            128 => TournamentStage.RoundOf128,
            _ => throw new ArgumentException("Unsupported seed count")
        };
    }

    private List<(string Home, string Away)> GetExpectedFirstRoundPairings(int seedCount)
    {
        switch (seedCount)
        {
            case 128:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 128"), ("Seed 64", "Seed 65"), ("Seed 32", "Seed 97"), ("Seed 33", "Seed 96"),
                    ("Seed 16", "Seed 113"), ("Seed 49", "Seed 80"), ("Seed 17", "Seed 112"), ("Seed 48", "Seed 81"),
                    ("Seed 8", "Seed 121"), ("Seed 57", "Seed 72"), ("Seed 25", "Seed 104"), ("Seed 40", "Seed 89"),
                    ("Seed 9", "Seed 120"), ("Seed 56", "Seed 73"), ("Seed 24", "Seed 105"), ("Seed 41", "Seed 88"),
                    ("Seed 4", "Seed 125"), ("Seed 61", "Seed 68"), ("Seed 29", "Seed 100"), ("Seed 36", "Seed 93"),
                    ("Seed 13", "Seed 116"), ("Seed 52", "Seed 77"), ("Seed 20", "Seed 109"), ("Seed 45", "Seed 84"),
                    ("Seed 5", "Seed 124"), ("Seed 60", "Seed 69"), ("Seed 28", "Seed 101"), ("Seed 37", "Seed 92"),
                    ("Seed 12", "Seed 117"), ("Seed 53", "Seed 76"), ("Seed 21", "Seed 108"), ("Seed 44", "Seed 85"),
                    ("Seed 2", "Seed 127"), ("Seed 63", "Seed 66"), ("Seed 31", "Seed 98"), ("Seed 34", "Seed 95"),
                    ("Seed 15", "Seed 114"), ("Seed 50", "Seed 79"), ("Seed 18", "Seed 111"), ("Seed 47", "Seed 82"),
                    ("Seed 7", "Seed 122"), ("Seed 58", "Seed 71"), ("Seed 26", "Seed 103"), ("Seed 39", "Seed 90"),
                    ("Seed 10", "Seed 119"), ("Seed 55", "Seed 74"), ("Seed 23", "Seed 106"), ("Seed 42", "Seed 87"),
                    ("Seed 3", "Seed 126"), ("Seed 62", "Seed 67"), ("Seed 30", "Seed 99"), ("Seed 35", "Seed 94"),
                    ("Seed 14", "Seed 115"), ("Seed 51", "Seed 78"), ("Seed 19", "Seed 110"), ("Seed 46", "Seed 83"),
                    ("Seed 6", "Seed 123"), ("Seed 59", "Seed 70"), ("Seed 27", "Seed 102"), ("Seed 38", "Seed 91"),
                    ("Seed 11", "Seed 118"), ("Seed 54", "Seed 75"), ("Seed 22", "Seed 107"), ("Seed 43", "Seed 86")
                };
            case 32:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 32"), ("Seed 16", "Seed 17"), ("Seed 8", "Seed 25"), ("Seed 9", "Seed 24"),
                    ("Seed 4", "Seed 29"), ("Seed 13", "Seed 20"), ("Seed 5", "Seed 28"), ("Seed 12", "Seed 21"),
                    ("Seed 2", "Seed 31"), ("Seed 15", "Seed 18"), ("Seed 7", "Seed 26"), ("Seed 10", "Seed 23"),
                    ("Seed 3", "Seed 30"), ("Seed 14", "Seed 19"), ("Seed 6", "Seed 27"), ("Seed 11", "Seed 22")
                };
            case 16:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 16"), ("Seed 8", "Seed 9"), ("Seed 4", "Seed 13"), ("Seed 5", "Seed 12"),
                    ("Seed 2", "Seed 15"), ("Seed 7", "Seed 10"), ("Seed 3", "Seed 14"), ("Seed 6", "Seed 11")
                };
            case 8:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 8"), ("Seed 4", "Seed 5"), ("Seed 2", "Seed 7"), ("Seed 3", "Seed 6")
                };
            case 4:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 4"), ("Seed 2", "Seed 3")
                };
            case 2:
                return new List<(string, string)>
                {
                    ("Seed 1", "Seed 2")
                };
            default:
                throw new ArgumentException("Unsupported seed count");
        }
    }
    
    private static IEnumerable<int> GetTestValues()
    {
        for (int i = 1; i <= 256; i++)
        {
            yield return i;
        }
    }
}