namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class MatchFlow : BaseDeletableEntity<int>
{
    public bool IsHome { get; set; }
    public int MatchId { get; set; }
    public int SuccessorMatchId { get; set; }
    
    public int TournamentId { get; set; }
    
    public Tournament Tournament { get; set; }
}