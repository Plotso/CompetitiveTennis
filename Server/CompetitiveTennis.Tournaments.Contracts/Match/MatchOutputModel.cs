﻿namespace CompetitiveTennis.Tournaments.Contracts.Match;

using CompetitiveTennis.Data.Models.Enums;
using Participant;
using Score;

public record MatchOutputModel(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    TournamentStage Stage,
    string? Details,
    EventStatus Status,
    MatchOutcome Outcome,
    IEnumerable<ScoreShortOutputModel> Scores,
    IEnumerable<ParticipantShortOutputModel> Participants);