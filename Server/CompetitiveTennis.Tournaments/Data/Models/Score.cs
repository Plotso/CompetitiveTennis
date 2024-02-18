namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Data.Models.Enums;

public class Score : BaseDeletableEntity<int>
{
//    public short Set { get; set; }
    
//    public short Game { get; set; }
    public int PeriodPointNumber { get; set; }
    
    public string Participant1Points { get; set; }
    
    public string Participant2Points { get; set; }
    
//    public EventStatus Status { get; set; }
    
    public MatchOutcome? PointWinner { get; set; }
    
//     public int MatchId { get; set; }
//     
//     public Match Match { get; set; }
    public int MatchPeriodId { get; set; }
    public MatchPeriod MatchPeriod { get; set; }
}