namespace CompetitiveTennis.Tournaments.Contracts.Team;

using Participant;

public record TeamOutputModel(int Id, string Name, IEnumerable<ParticipantShortOutputModel> Participants);