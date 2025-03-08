namespace CompetitiveTennis.Tournaments.Models.TournamentDrawGenerator;

public record Seed(int Id, string Name)
{
    public bool IsQualificationPlayer { get; set; }
}