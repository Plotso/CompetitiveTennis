namespace CompetitiveTennis.Tournaments.Gateway.Models.Participant;

using Match;

public record ParticipantMatchOutputModel(ParticipantShortOutputModel Participant, MatchOutputModel match);