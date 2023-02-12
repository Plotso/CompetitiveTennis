namespace CompetitiveTennis.Identity.Models;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants;

public class UserInputModel
{
    [EmailAddress]
    [Required]
    [MinLength(MinEmailLength)]
    [MaxLength(MaxEmailLength)]
    public string LoginInfo { get; set; }

    [Required]
    public string Password { get; set; }
    
    public bool EmailLogin { get; set; }
}