namespace CompetitiveTennis.Tournaments.Contracts.Account;

using System.ComponentModel.DataAnnotations;

using static Constants.Accounts;

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