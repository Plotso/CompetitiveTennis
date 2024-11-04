namespace CompetitiveTennis.Tournaments.Contracts.Match;

using CompetitiveTennis.Data.Models.Enums;
using MatchPeriod;
using Participant;

public record MatchOutputModel(
    int Id,
    int TournamentId,
    DateTime StartDate,
    DateTime EndDate,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    TournamentStage Stage,
    string? Details,
    EventStatus Status,
    MatchOutcome Outcome,
    IEnumerable<MatchPeriodOutputModel> MatchPeriods,
    IEnumerable<ParticipantShortOutputModel> Participants);