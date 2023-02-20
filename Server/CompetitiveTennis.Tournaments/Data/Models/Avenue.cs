namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Avenue : BaseDeletableEntity<int>
{
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public string Details { get; set; }
    
    
    public bool IsVerified { get; set; }
    
    public bool IsActive { get; set; }

    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}