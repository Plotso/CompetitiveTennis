namespace CompetitiveTennis.Tournaments.Contracts.MatchPeriod.Score;

using System.ComponentModel.DataAnnotations;
using CompetitiveTennis.Data.Models.Enums;
using static Constants;

public record ScoreInputModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int PeriodPointNumber { get; set; }
    
    [Required]
    [MaxLength(MaxScorePointsValueLength)]
    public string Participant1Points { get; set; }
    
    [Required]
    [MaxLength(MaxScorePointsValueLength)]
    public string Participant2Points { get; set; }
    [Required]
    public MatchOutcome PointWinner { get; set; }
}