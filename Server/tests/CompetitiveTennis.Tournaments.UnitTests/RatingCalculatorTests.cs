namespace CompetitiveTennis.Tournaments.UnitTests;

using CompetitiveTennis.Data.Models.Enums;
using Configurations;
using Data;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Models.MatchOutcomeHandler.RatingCalculations;
using Moq;
using Services.BL;

[TestFixture]
public class RatingCalculatorTests
{
    private Mock<IOptionsMonitor<RatingCalculatorConfiguration>> _mockOptions;
    private RatingCalculatorMoq _ratingCalculator;

    [SetUp]
    public void SetUp()
    {
        _mockOptions = new Mock<IOptionsMonitor<RatingCalculatorConfiguration>>();
        _mockOptions.Setup(x => x.CurrentValue)
            .Returns(new RatingCalculatorConfiguration { DoublesRatingCalculationEnabled = true });

        _ratingCalculator = new RatingCalculatorMoq(_mockOptions.Object);
    }

    [Test]
    public void CalculateRatings_ShouldThrowArgumentException_WhenMatchResultIsNull()
    {
        // Act
        Action act = () => _ratingCalculator.CalculateRatings(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Match participants must not be null or empty in order to Calculate new ratings");
    }

    [Test]
    public void CalculateRatings_ShouldThrowArgumentException_WhenParticipantsAreEmpty()
    {
        // Arrange
        var matchResult = CreateStandardSinglesMatchResult() with
        {
            Participants = Array.Empty<ParticipantRatingOutputModel>()
        };

        // Act
        Action act = () => _ratingCalculator.CalculateRatings(matchResult);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Match participants must not be null or empty in order to Calculate new ratings");
    }

    [Test]
    public void HandleSinglesRating_ShouldReturnCorrectRatings_ForStandardMatch()
    {
        // Arrange
        var matchResult = CreateStandardSinglesMatchResult();

        // Act
        var result = _ratingCalculator.CalculateRatings(matchResult, isDoubles: false);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(1, 1639));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(2, 1460));
    }

    [Test]
    public void HandleDoublesRating_ShouldReturnEmpty_WhenDoublesRatingCalculationIsDisabled()
    {
        // Arrange
        _mockOptions.Setup(x => x.CurrentValue)
            .Returns(new RatingCalculatorConfiguration { DoublesRatingCalculationEnabled = false });

        var matchResult = CreateStandardDoublesMatchResult();

        // Act
        var result = _ratingCalculator.CalculateRatings(matchResult, isDoubles: true);

        // Assert
        result.Should().BeEmpty();
    }
    
    [Test]
    public void HandleDoublesRating_ShouldReturnCorrectRatings_ForStandardMatch()
    {
        // Arrange
        var matchResult = CreateStandardDoublesMatchResult();

        // Act
        var result = _ratingCalculator.CalculateRatings(matchResult, isDoubles: true);

        // Assert
        result.Should().HaveCount(4);
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(1, 1513));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(2, 1513));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(3, 1387));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(4, 1387));
    }
    
    [Test]
    public void CalculateMarginOfVictory_ShouldReturnCorrectMultiplier_ForStandardMatch([Values] bool isHomeWinner)
    {
        // Arrange
        var winnerResult = new TennisResultInfo (SetsWon: 2, GamesWon: 12, PointsWon: 60 );
        var loserResult = new TennisResultInfo (SetsWon: 1, GamesWon: 9, PointsWon: 50 );

        // Act
        var result = _ratingCalculator.CalculateMarginOfVictoryMoq(
            homeResult: isHomeWinner ? winnerResult : loserResult,
            awayResult: isHomeWinner ? loserResult : winnerResult);

        // Assert
        result.Should().BeApproximately(3.432, 0.01); // Example expected multiplier
    }
    
    [Test]
    public void CalculateSetMultiplier_ShouldReturnCorrectValue_ForBestOfThree([Values] bool isHomeWinner)
    {
        // Arrange
        byte winnerSetsWon = 2, loserSetsWon = 1;

        // Act
        var result = _ratingCalculator.CalculateSetMultiplierMoq(
            homeSetsWon: isHomeWinner ? winnerSetsWon : loserSetsWon,
            awaySetsWon: isHomeWinner ? loserSetsWon : winnerSetsWon);

        // Assert
        result.Should().Be(1.2);
    }
    
    [TestCase(2, 1, false, 1.2)] // BO3 close match
    [TestCase(1, 2, false, 1.2)] // BO3 close match
    [TestCase(2, 0, false, 1.0)] // BO3 dominant win
    [TestCase(0, 2, false, 1.0)] // BO3 dominant win
    [TestCase(3, 0, true, 1.5)]  // BO5 dominant match
    [TestCase(0, 3, true, 1.5)]  // BO5 dominant match
    [TestCase(3, 1, true, 1.2)]  // BO5 moderate win
    [TestCase(1, 3, true, 1.2)]  // BO5 moderate win
    [TestCase(3, 2, true, 1.0)]  // BO5 narrow win
    [TestCase(2, 3, true, 1.0)]  // BO5 narrow win
    public void CalculateSetMultiplierMoq_ShouldReturnCorrectValue(byte homeSetsWon, byte awaySetsWon, bool isBestOfFive, double expectedMultiplier)
    {
        // Arrange
        var calculator = new RatingCalculatorMoq(MockOptionsMonitor());
    
        // Act
        var result = calculator.CalculateSetMultiplierMoq(homeSetsWon, awaySetsWon);
    
        // Assert
        result.Should().Be(expectedMultiplier);
    }

    [Test]
    public void CalculateSetMultiplierMoq_WithDraw_ShouldReturnMultiplier0([Values(0,1,2,3)] byte setsWon, [Values] bool isBestOfFive)
    {
        // Arrange
        var calculator = new RatingCalculatorMoq(MockOptionsMonitor());
    
        // Act
        var result = calculator.CalculateSetMultiplierMoq(setsWon, setsWon);
    
        // Assert
        result.Should().Be(0);
    }

    [Test]
    public void DistributeDoublesRatingChanges_ShouldDistributeRatingsCorrectly()
    {
        // Arrange
        var homeParticipant = new ParticipantRatingOutputModel(
            Id: 1,
            Specifier: DataConstants.ParticipantSpecifiers.Home,
            IsGuest: false,
            Players: new List<AccountRatingOutputModel>
            {
                new(1, 1500, 1500),
                new(2, 1500, 1500)
            }
        );
        var awayParticipant = new ParticipantRatingOutputModel(
            Id: 1,
            Specifier: DataConstants.ParticipantSpecifiers.Home,
            IsGuest: false,
            Players: new List<AccountRatingOutputModel>
            {
                new(3, 1400, 1400),
                new(4, 1400, 1400)
            }
        ); 
        

        // Act
        var result = _ratingCalculator.DistributeDoublesRatingChangesMoq(
            2, 3000, 2800, 3100, 2700, homeParticipant, awayParticipant);

        // Assert
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(1, 1550));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(2, 1550));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(3, 1350));
        result.Should().ContainEquivalentOf(new NewRatingPerAccount(4, 1350));
    }

    private MatchResultSummaryWithRatings CreateStandardSinglesMatchResult()
    {
        return new MatchResultSummaryWithRatings
        (
            Id: 0,
            Outcome: MatchOutcome.ParticipantOne,
            HomeSideResult: new TennisResultInfo (SetsWon: 2, GamesWon: 12, PointsWon: 60),
            AwaySideResult: new TennisResultInfo (SetsWon: 0, GamesWon: 5, PointsWon: 30),
            Participants: new List<ParticipantRatingOutputModel>
            {
                new
                (
                    Id: 1,
                    Specifier: DataConstants.ParticipantSpecifiers.Home,
                    IsGuest: false,
                    Players: new List<AccountRatingOutputModel> { new(1, 1600, 1600) }
                ),
                new 
                (
                    Id: 2,
                    Specifier: DataConstants.ParticipantSpecifiers.Away,
                    IsGuest: false,
                    Players: new List<AccountRatingOutputModel> { new(2, 1500, 1500) }
                )
            }
        );
    }

    private MatchResultSummaryWithRatings CreateStandardDoublesMatchResult()
    {
        return new MatchResultSummaryWithRatings
        (
            Id: 0,
            Outcome: MatchOutcome.ParticipantOne,
            HomeSideResult: new TennisResultInfo (SetsWon: 2, GamesWon: 12, PointsWon: 60),
            AwaySideResult: new TennisResultInfo (SetsWon: 0, GamesWon: 5, PointsWon: 30),
            Participants: new List<ParticipantRatingOutputModel>
            {
                new
                (
                    Id: 1,
                    Specifier: DataConstants.ParticipantSpecifiers.Home,
                    IsGuest: false,
                    Players: new List<AccountRatingOutputModel>
                    {
                        new(1, 1500, 1500),
                        new(2, 1500, 1500)
                    }
                ),
                new 
                (
                    Id: 2,
                    Specifier: DataConstants.ParticipantSpecifiers.Away,
                    IsGuest: false,
                    Players: new List<AccountRatingOutputModel>
                    {
                        new(3, 1400, 1400),
                        new(4, 1400, 1400)
                    }
                )
            }
        );
    }

    private static IOptionsMonitor<RatingCalculatorConfiguration> MockOptionsMonitor()
    {
        var mockOptions = new Mock<IOptionsMonitor<RatingCalculatorConfiguration>>();
        mockOptions.Setup(x => x.CurrentValue).Returns(new RatingCalculatorConfiguration
        {
            DoublesRatingCalculationEnabled = true
        });
        return mockOptions.Object;
    }

}