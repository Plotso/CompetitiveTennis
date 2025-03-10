﻿namespace CompetitiveTennis.Tournaments.Models.TournamentDrawGenerator;

using CompetitiveTennis.Data.Models.Enums;

public record MatchGeneratorOutput()
{
    public int Id { get; set; }
    public int PlayOrderNumber { get; set; }
    public string HomeSeedPlaceholder { get; set; }
    public string AwaySeedPlaceholder { get; set; }
    public Seed HomeSeed { get; set; }
    public Seed AwaySeed { get; set; }
    
    public int? HomePrevMatch { get; set; }
    public int? AwayPrevMatch { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public TournamentStage TournamentStage { get; set; }
}