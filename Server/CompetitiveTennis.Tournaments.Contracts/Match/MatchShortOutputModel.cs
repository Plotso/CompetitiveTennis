namespace CompetitiveTennis.Tournaments.Contracts.Match;

using CompetitiveTennis.Data.Models.Enums;
using Participant;
using Score;

public record MatchShortOutputModel(
    int Id,
    DateTime StartDate,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    TournamentStage Stage,
    string? Details,
    EventStatus Status,
    MatchOutcome Outcome,
    OutcomeCondition? OutcomeCondition,
    int? HomePredecesorMatchId,
    int? AwayPredecesorMatchId,
    ParticipantInfo? HomeParticipant,
    ParticipantInfo? AwayParticipant,
    IEnumerable<ScoreShortOutputModel> Scores);