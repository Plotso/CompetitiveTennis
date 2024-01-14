namespace CompetitiveTennis.Tournaments.Models.Match;

using System.ComponentModel.DataAnnotations;
using CompetitiveTennis.Data.Models.Enums;

public record MatchInputModel
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Participant1Id { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Participant2Id { get; set; }
    
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