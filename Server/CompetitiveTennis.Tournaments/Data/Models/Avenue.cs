namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;
using Enums;

public class Avenue : BaseDeletableEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public string Details { get; set; }
    
    
    public bool IsVerified { get; set; }
    
    public bool IsActive { get; set; }

    public string Courts { get; set; }

    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}