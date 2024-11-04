namespace CompetitiveTennis.Tournaments.Contracts.MatchPeriod.Score;

using CompetitiveTennis.Data.Models.Enums;

public record ScoreShortOutputModel(int Id, int PeriodPointNumber, string Participant1Points, string Participant2Points, MatchOutcome PointWinner, bool IsWinningPoint);