namespace CompetitiveTennis.Data.Models.Interfaces;

public interface IAuditInfo
{
    DateTime CreatedOn { get; set; }

    DateTime? ModifiedOn { get; set; }
}