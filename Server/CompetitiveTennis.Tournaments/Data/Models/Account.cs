namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Account : BaseDeletableEntity<int>
{
    public string UserId { get; set; }
    
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PlayerRating { get; set; }

    public ICollection<AccountParticipant> Participations { get; set; } = new List<AccountParticipant>();

    public ICollection<Tournament> OrganisedTournaments { get; set; } = new List<Tournament>();
}