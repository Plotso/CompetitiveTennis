namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;
using Enums;

public class Tournament : BaseDeletableEntity<int>
{
    public string Title { get; set; }
    
    public string Rules { get; set; }
    public string Description { get; set; }

    public TournamentType Type { get; set; }
    
    public Surface Surface { get; set; }
    
    public decimal? EntryFee { get; set; }
    
    public decimal? Prize { get; set; }
    
    public short CourtsAvailable { get; set; }
    
    public short MinParticipants { get; set; }
    
    public short MaxParticipants { get; set; }

    public short? MatchWonPoints { get; set; }
    public short? SetWonPoints { get; set; }
    public short? GameWonPoints { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public int AvenueId { get; set; }
    
    public Avenue Avenue { get; set; }
    
    public int OrganiserId { get; set; }
    
    public Account Organiser { get; set; }

    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}