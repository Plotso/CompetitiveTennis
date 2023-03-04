namespace CompetitiveTennis.Tournaments.Models.Score;

using Data.Models.Enums;

public record ScoreShortOutputModel(int Id, short Set, short Game, string Participant1Points, string Participant2Points, EventStatus Status);