namespace CompetitiveTennis.Tournaments.Gateway.Models.Score;

using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;

public record ScoreInputModel
{
    public short Set { get; set; }
    
    public short Game { get; set; }
    
    [Required]
    public string Participant1Points { get; set; }
    
    [Required]
    public string Participant2Points { get; set; }
    
    [Required]
    public EventStatus Status { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int MatchId { get; set; }
}