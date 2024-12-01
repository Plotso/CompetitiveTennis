namespace CompetitiveTennis.Tournaments.Models.MatchOutcomeHandler.RatingCalculations;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Tournaments.Contracts.MatchPeriod;

public record SlimMatchOutputModel(
    int Id,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    EventStatus Status,
    MatchOutcome Outcome,
    IEnumerable<MatchPeriodOutputModel> MatchPeriods,
    IEnumerable<ParticipantRatingOutputModel> Participants);