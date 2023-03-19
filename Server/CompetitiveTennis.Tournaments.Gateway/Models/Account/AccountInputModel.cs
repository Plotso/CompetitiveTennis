namespace CompetitiveTennis.Tournaments.Gateway.Models.Account;

using System.ComponentModel.DataAnnotations;

public record AccountInputModel
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
}