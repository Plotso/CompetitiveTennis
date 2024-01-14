namespace CompetitiveTennis.Tournaments.Contracts.Tournament;

using Data.Models.Enums;

public record TournamentShortInfoOutput(
    int Id,
    string Title,
    TournamentType Type,
    Surface Surface,
    decimal? EntryFee,
    decimal? Prize,
    DateTime StartDate,
    DateTime EndDate);