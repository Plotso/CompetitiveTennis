namespace CompetitiveTennis.Tournaments.Gateway.Models.Team;

using Participant;

public record TeamOutputModel(int Id, string Name, IEnumerable<ParticipantShortOutputModel> Participants);