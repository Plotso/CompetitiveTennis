namespace CompetitiveTennis.Tournaments.Gateway.Models.Participant;

public record MultiParticipantInputModel(ParticipantInputModel ParticipantInfo, IEnumerable<int> Accounts, bool IncludeCurrentUser);