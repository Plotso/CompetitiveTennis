namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;
using CompetitiveTennis.Data.Models.Enums;

public class Match : BaseDeletableEntity<int>
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public short? MatchWonPoints { get; set; }
    public short? SetWonPoints { get; set; }
    public short? GameWonPoints { get; set; }
    
    public TournamentStage Stage { get; set; }
    
    public string? Details { get; set; }
    
    public EventStatus Status { get; set; }
    public MatchOutcome? Outcome { get; set; }
    
    public int TournamentId { get; set; }
    
    public Tournament Tournament { get; set; }
    public ICollection<Score> Scores { get; set; }
    
    public ICollection<ParticipantMatch> Participants { get; set; }
}