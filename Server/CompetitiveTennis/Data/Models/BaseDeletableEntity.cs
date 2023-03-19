namespace CompetitiveTennis.Data.Models;

using Interfaces;

public class BaseDeletableEntity<TKey> : BaseModel<TKey>, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    
    public string? DeletedBy { get; set; }

    public DateTime? DeletedOn { get; set; }
}