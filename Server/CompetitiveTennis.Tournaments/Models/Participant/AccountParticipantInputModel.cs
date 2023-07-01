namespace CompetitiveTennis.Tournaments.Models.Participant;

using System.ComponentModel.DataAnnotations;

public record AccountParticipantInputModel
{
    [Range(1, int.MaxValue)]
    public required int AccountId { get; init; }
    public required ParticipantInputModel ParticipantInput { get; init; }
}