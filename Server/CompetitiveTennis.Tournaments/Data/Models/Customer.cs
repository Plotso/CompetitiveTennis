namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Customer : BaseDeletableEntity<int>
{
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PlayerRating { get; set; }
}