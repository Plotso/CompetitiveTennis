namespace CompetitiveTennis.Tournaments.Models.Participant;

using Account;
using Match;
using Team;
using Tournament;

public record ParticipantOutputModel(
    int Id,
    string? Name,
    int? Points,
    bool IsGuest,
    TournamentShortInfoOutput Tournament,
    TeamShortOutputModel? Team,
    IEnumerable<AccountOutputModel> Players,
    IEnumerable<MatchOutputModel> Matches);