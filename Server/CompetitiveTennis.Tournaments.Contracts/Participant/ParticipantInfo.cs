namespace CompetitiveTennis.Tournaments.Contracts.Participant;

using Account;
using Team;

public record ParticipantInfo(int Id, bool HasGuest, string Name, IEnumerable<AccountShortOutputModel> Players, string Specifier, TeamShortOutputModel? Team);