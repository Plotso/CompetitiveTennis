namespace CompetitiveTennis.Tournaments.Contracts.Match;

using Data.Models.Enums;
using MatchPeriod;

public record MatchOutcomeInputModel(OutcomeCondition? OutcomeCondition, IEnumerable<MatchPeriodInputModel> matchPeriods);