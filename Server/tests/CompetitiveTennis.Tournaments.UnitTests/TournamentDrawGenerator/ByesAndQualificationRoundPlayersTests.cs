namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator;

using FluentAssertions;

[TestFixture]
public class ByesAndQualificationRoundPlayersTests
{
    
    [TestCase(7, 1)]
    [TestCase(8, 8)]
    [TestCase(9, 7)]
    [TestCase(10, 6)]
    [TestCase(15, 1)]
    [TestCase(16, 16)]
    [TestCase(17, 15)]
    [TestCase(32, 32)]
    [TestCase(48, 16)]
    [TestCase(64, 64)]
    [TestCase(82, 46)]
    [TestCase(96, 32)]
    public void GetByesCount_ShouldReturnCorrectNumberOfByes(int players, int expectedByes)
    {
        var closestPower = MatchesGeneratorMoq.ClosestPowerOfTwoMoq(players);
        var result = MatchesGeneratorMoq.GetByesCountMoq(players, closestPower);
        result.Should().Be(expectedByes);
    }
    
    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(3, 2)]
    [TestCase(4, 4)]
    [TestCase(5, 4)]
    [TestCase(7, 4)]
    [TestCase(8, 8)]
    [TestCase(9, 8)]
    [TestCase(15, 8)]
    [TestCase(16, 16)]
    [TestCase(17, 16)]
    [TestCase(32, 32)]
    [TestCase(48, 32)]
    [TestCase(64, 64)]
    [TestCase(82, 64)]
    [TestCase(96, 64)]
    public void ClosestPowerOfTwoMoq_ShouldReturnCorrectPowerOfTwo(int number, int expected)
    {
        var result = MatchesGeneratorMoq.ClosestPowerOfTwoMoq(number);
        result.Should().Be(expected);
    }

    [Test]
    public void ClosestPowerOfTwoMoq_WithZero_ShouldThrowArgumentException()
    {
        Action act = () => MatchesGeneratorMoq.ClosestPowerOfTwoMoq(0);
        act.Should().Throw<ArgumentException>().WithMessage("Number of players must be positive.*");
    }

    [Test]
    public void ClosestPowerOfTwoMoq_WithNegative_ShouldThrowArgumentException()
    {
        Action act = () => MatchesGeneratorMoq.ClosestPowerOfTwoMoq(-1);
        act.Should().Throw<ArgumentException>().WithMessage("Number of players must be positive.*");
    }
}