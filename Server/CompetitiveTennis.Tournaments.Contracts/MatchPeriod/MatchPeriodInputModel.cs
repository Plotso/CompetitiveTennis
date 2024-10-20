namespace CompetitiveTennis.Tournaments.Contracts.MatchPeriod;

using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;
using Score;

public record MatchPeriodInputModel
{
    public short Set { get; set; }
    
    public short Game { get; set; }
    
    [Required]
    public EventStatus Status { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int MatchId { get; set; }
    
    public EventActor Server { get; set; }
    public MatchOutcome Winner { get; set; }
    
    public bool IsTiebreak { get; set; }
    
    public IEnumerable<ScoreInputModel> Scores { get; set; }
}