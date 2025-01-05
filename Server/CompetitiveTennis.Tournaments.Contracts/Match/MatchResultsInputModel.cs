namespace CompetitiveTennis.Tournaments.Contracts.Match;

using MatchPeriod;

public record MatchResultsInputModel
{
    public bool IsEnded { get; init; }
    public MatchPeriodInputModel[] MatchPeriods { get; init; }
}