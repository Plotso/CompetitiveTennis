namespace CompetitiveTennis.Tournaments.Gateway.Models.Match;

using Data.Models.Enums;
using Participant;
using Score;

public record MatchOutputModel(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    int Participant1Id,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    TournamentStage Stage,
    string? Details,
    EventStatus Status,
    MatchOutcome Outcome,
    IEnumerable<ScoreShortOutputModel> Scores,
    IEnumerable<ParticipantShortOutputModel> Participants);