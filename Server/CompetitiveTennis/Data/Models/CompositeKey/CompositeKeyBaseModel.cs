namespace CompetitiveTennis.Data.Models.CompositeKey;

using Interfaces;

public class CompositeKeyBaseModel : IAuditInfo
{
    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}