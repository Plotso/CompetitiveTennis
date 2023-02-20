namespace CompetitiveTennis.Data.Models.CompositeKey;

using Interfaces;

public class CompositeKeyDeletableEntity : CompositeKeyBaseModel, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    
    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }
}