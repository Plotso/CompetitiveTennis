namespace CompetitiveTennis.Tournaments.Models.Participant;

using Match;

public record ParticipantMatchOutputModel(ParticipantShortOutputModel Participant, MatchOutputModel match);