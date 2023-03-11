namespace CompetitiveTennis.Tournaments.Gateway.Models.Participant;

using Account;

public record ParticipantShortOutputModel(int Id, string? Name, int? Points, bool IsGuest, IEnumerable<AccountShortOutputModel> Players);