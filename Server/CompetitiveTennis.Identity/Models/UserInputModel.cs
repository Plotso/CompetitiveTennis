namespace CompetitiveTennis.Identity.Models;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants;

public class UserInputModel
{
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxUsernameLength)]
    public string LoginInfo { get; set; }

    [Required]
    public string Password { get; set; }
    
    public bool EmailLogin { get; set; }
}