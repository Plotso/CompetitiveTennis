namespace CompetitiveTennis.Tournaments.Data.Models;

using CompetitiveTennis.Data.Models.CompositeKey;

public class AccountParticipant : CompositeKeyDeletableEntity
{
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public int ParticipantId { get; set; }
    public Participant Participant { get; set; }
}