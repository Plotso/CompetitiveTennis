namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Team : BaseDeletableEntity<int>
{
    public string Name { get; set; }

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
}