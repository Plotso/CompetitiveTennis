namespace CompetitiveTennis.Tournaments.Data.Models;

using System.Collections.ObjectModel;
using CompetitiveTennis.Data.Models;

public class Account : BaseDeletableEntity<int>
{
    public string UserId { get; set; }
    
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PlayerRating { get; set; }

    public Collection<AccountParticipant> Participations { get; set; } = new();
}