namespace CompetitiveTennis.Identity.Data.Models;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime? LastLogin { get; set; }
}