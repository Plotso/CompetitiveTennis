namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models.CompositeKey;

public class ParticipantMatch : CompositeKeyDeletableEntity
{
    public int ParticipantId { get; set; }
    public Participant Participant { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; }
}