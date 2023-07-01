namespace CompetitiveTennis.Tournaments.Models.Participant;

using System.ComponentModel.DataAnnotations;

public record ParticipantInputModel
{
    public string? Name { get; init; }
    
    public int? Points { get; init; }
    
    public bool IsGuest { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int TournamentId { get; init; }
    
    public int? TeamId { get; init; }
};