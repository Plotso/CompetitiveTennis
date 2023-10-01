namespace CompetitiveTennis.Tournaments.Gateway.Models.Match;

using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;

public record MatchInputModel
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int HomeParticipantId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int AwayParticipantId { get; set; }
    
    public short? MatchWonPoints { get; set; }
    public short? SetWonPoints { get; set; }
    public short? GameWonPoints { get; set; }
    
    [Required]
    public TournamentStage Stage { get; set; }
    
    public string? Details { get; set; }
    
    [Required]
    public EventStatus Status { get; set; }
    
    public MatchOutcome? Outcome { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int TournamentId { get; set; }
};