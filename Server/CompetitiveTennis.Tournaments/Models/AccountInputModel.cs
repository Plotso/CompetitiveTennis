namespace CompetitiveTennis.Tournaments.Models;

using System.ComponentModel.DataAnnotations;

using static Data.DataConstants.Accounts;

public record AccountInputModel
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