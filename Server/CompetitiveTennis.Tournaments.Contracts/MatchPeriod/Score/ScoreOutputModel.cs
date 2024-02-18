namespace CompetitiveTennis.Tournaments.Contracts.MatchPeriod.Score;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Tournaments.Contracts.Match;

public record ScoreOutputModel(int Id, int PeriodPointNumber, string Participant1Points, string Participant2Points, MatchOutcome PointWinner, int MatchPeriodId);