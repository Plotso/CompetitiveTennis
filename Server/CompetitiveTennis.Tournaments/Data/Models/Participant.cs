﻿namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models;

public class Participant : BaseDeletableEntity<int>
{
    public string? Name { get; set; }
    
    public int? Points { get; set; }
    
    public bool IsGuest { get; set; }
    public int? TournamentId { get; set; }
    public Tournament Tournament { get; set; }
    
    public int? TeamId { get; set; }
    
    public Team? Team { get; set; }

    public ICollection<AccountParticipant> Players { get; set; } = new List<AccountParticipant>();

    public ICollection<ParticipantMatch> Matches { get; set; } = new List<ParticipantMatch>();
}