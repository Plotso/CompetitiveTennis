namespace CompetitiveTennis.Tournaments.Models.Tournament;

using Account;
using Avenue;
using Data.Models.Enums;

public record TournamentOutputModel(
    int Id,
    string Title,
    string Rules,
    string Description,
    TournamentType Type,
    Surface Surface,
    decimal? EntryFee,
    decimal? Prize,
    short CourtsAvailable,
    short MinParticipants,
    short MaxParticipants,
    short? MatchWonPoints,
    short? SetWonPoints,
    short? GameWonPoints,
    DateTime StartDate,
    DateTime EndDate,
    DateTime CreatedOn,
    DateTime ModifiedOn,
    AvenueShortOutputModel Avenue,
    AccountOutputModel Organiser);