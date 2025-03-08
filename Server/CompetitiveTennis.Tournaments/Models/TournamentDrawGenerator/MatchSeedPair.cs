namespace CompetitiveTennis.Tournaments.Models.TournamentDrawGenerator;

public class MatchSeedPair
{
    public string HomeSeedPlaceholder { get; set; }
    public string AwaySeedPlaceholder { get; set; }
    public Seed HomeSeed { get; set; }
    public Seed AwaySeed { get; set; }
    
    public int? HomePrevMatch { get; set; }
    public int? AwayPrevMatch { get; set; }
}