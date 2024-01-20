namespace CompetitiveTennis.Tournaments.Contracts.Score;

using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;
using static Constants;

public record ScoreInputModel
{
    public short Set { get; set; }
    
    public short Game { get; set; }
    
    [Required]
    [MaxLength(MaxScorePointsValueLength)]
    public string Participant1Points { get; set; }
    
    [Required]
    [MaxLength(MaxScorePointsValueLength)]
    public string Participant2Points { get; set; }
    
    [Required]
    public EventStatus Status { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int MatchId { get; set; }
    
    [Required]
    public MatchOutcome PointWinner { get; set; }
}