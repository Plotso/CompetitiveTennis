namespace CompetitiveTennis.Tournaments.Contracts.MatchPeriod;

using Data.Models.Enums;
using Score;

public record MatchPeriodOutputModel(
    int Id,
    short Set,
    short Game,
    EventStatus Status,
    MatchOutcome Winner,
    EventActor Server,
    IEnumerable<ScoreShortOutputModel> Scores);