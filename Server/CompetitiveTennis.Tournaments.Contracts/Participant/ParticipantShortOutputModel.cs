namespace CompetitiveTennis.Tournaments.Contracts.Participant;

using Account;

public record ParticipantShortOutputModel(int Id, string? Name, int? Points, bool IsGuest, IEnumerable<AccountShortOutputModel> Players, string Specifier);