namespace CompetitiveTennis.Tournaments.Models.TournamentDrawGenerator;

public class MatchSeedPair
{
    public string HomePlayer { get; set; }
    public string AwayPlayer { get; set; }
    public Seed HomeSeed { get; set; }
    public Seed AwaySeed { get; set; }
    
    public int? HomePrevMatch { get; set; }
    public int? AwayPrevMatch { get; set; }
}