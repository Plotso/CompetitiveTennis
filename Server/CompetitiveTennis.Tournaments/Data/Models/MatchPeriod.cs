namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Data.Models.Enums;

public class MatchPeriod : BaseDeletableEntity<int>
{
    public short Set { get; set; }
    
    public short Game { get; set; }
    
    public EventStatus Status { get; set; }
    
    public EventActor Server { get; set; }
    
    public MatchOutcome? Winner { get; set; }
    
    public int MatchId { get; set; }
    
    public Match Match { get; set; }
    
    public ICollection<Score> Scores { get; set; }
}