namespace CompetitiveTennis.Data.Models;

using System.ComponentModel.DataAnnotations;
using Interfaces;

public class BaseModel<TKey> : IAuditInfo
{
    [Key]
    public TKey Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}