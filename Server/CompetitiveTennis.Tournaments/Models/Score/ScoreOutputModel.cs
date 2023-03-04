namespace CompetitiveTennis.Tournaments.Models.Score;

using Data.Models.Enums;
using Match;

public record ScoreOutputModel(int Id, short Set, short Game, string Participant1Points, string Participant2Points, EventStatus Status, MatchOutputModel Match);