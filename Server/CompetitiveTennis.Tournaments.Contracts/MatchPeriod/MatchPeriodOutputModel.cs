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
    bool IsTiebreak,
    IEnumerable<ScoreShortOutputModel> Scores);