namespace CompetitiveTennis.Tournaments.Models;

using CompetitiveTennis.Data.Models.Enums;
using Contracts.Account;
using Contracts.Avenue;
using CompetitiveTennis.Tournaments.Contracts.Match;
using Contracts.Participant;
using Contracts.Tournament;

public record FullTournamentOutputModel(
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
    AccountOutputModel Organiser,
    IEnumerable<ParticipantShortOutputModel> Participants,
    IEnumerable<MatchOutputModel> Matches,
    IEnumerable<MatchFlowOutput> MatchFlows) : TournamentOutputModel(
    Id, Title, Rules, Description, Type, Surface, EntryFee, Prize, CourtsAvailable, MinParticipants, MaxParticipants, MatchWonPoints, SetWonPoints, GameWonPoints, StartDate, EndDate, CreatedOn, ModifiedOn,
    Avenue, Organiser, Participants, Matches);