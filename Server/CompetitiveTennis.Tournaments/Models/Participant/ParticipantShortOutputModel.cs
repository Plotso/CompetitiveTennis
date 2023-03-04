namespace CompetitiveTennis.Tournaments.Models.Participant;

public record ParticipantShortOutputModel(int Id, string? Name, int? Points, bool IsGuest);