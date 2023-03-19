namespace CompetitiveTennis.Tournaments.Gateway.Models.Participant;

using System.ComponentModel.DataAnnotations;

public record ParticipantInputModel
{
    public string? Name { get; set; }
    
    public int? Points { get; set; }
    
    public bool IsGuest { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int TournamentId { get; set; }
    
    public int? TeamId { get; set; }
};