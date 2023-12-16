namespace CompetitiveTennis.Tournaments.Contracts.Participant;

using Match;

public record ParticipantMatchOutputModel(ParticipantShortOutputModel Participant, MatchOutputModel match);