namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Avenue : BaseDeletableEntity<int>
{
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public string City { get; set; }
    
    public string Country { get; set; }
    
    public string Details { get; set; }
    
    
    public bool IsVerified { get; set; }
    
    public bool IsActive { get; set; }

    public string Courts { get; set; }
    
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}