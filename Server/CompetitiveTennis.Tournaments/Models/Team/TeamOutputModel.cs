namespace CompetitiveTennis.Tournaments.Models.Team;

using Participant;

public record TeamOutputModel(int Id, string Name, IEnumerable<ParticipantShortOutputModel> Participants);