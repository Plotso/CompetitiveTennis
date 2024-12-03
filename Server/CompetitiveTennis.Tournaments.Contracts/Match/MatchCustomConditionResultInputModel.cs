namespace CompetitiveTennis.Tournaments.Contracts.Match;

using Data.Models.Enums;

public record MatchCustomConditionResultInputModel
{
    public OutcomeCondition OutcomeCondition { get; set; }
    public MatchOutcome MatchOutcome { get; set; }
};