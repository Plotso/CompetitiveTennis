namespace CompetitiveTennis.Identity.Models;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants;

public record ChangeNamesInputModel
{
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string FirstName { get; set; }
    
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string LastName { get; set; }
}