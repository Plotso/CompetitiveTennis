namespace CompetitiveTennis.Tournaments.Contracts.Account;

using Participant;
using Tournament;

public record AccountOutputModel(
    int Id,
    string Username,
    string FirstName,
    string LastName,
    int PlayerRating,
    int DoublesRating,
    IEnumerable<ParticipantShortOutputModel> Participations,
    IEnumerable<TournamentShortInfoOutput> OrganisedTournaments);