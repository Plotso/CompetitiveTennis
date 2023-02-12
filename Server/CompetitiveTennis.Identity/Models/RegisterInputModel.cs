namespace CompetitiveTennis.Identity.Models;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants;

public record RegisterInputModel
{
    [EmailAddress]
    [Required]
    [MinLength(MinEmailLength)]
    [MaxLength(MaxEmailLength)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxUsernameLength)]
    public string Username { get; set; }
    
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string FirstName { get; set; }
    
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string LastName { get; set; }

    [Required]
    public string Password { get; set; }
}