namespace CompetitiveTennis.Tournaments.Contracts.Participant;

public record MultiParticipantInputModel(ParticipantInputModel ParticipantInfo, IEnumerable<int> Accounts, bool IncludeCurrentUser);