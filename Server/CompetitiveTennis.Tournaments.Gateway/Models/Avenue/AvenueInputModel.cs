namespace CompetitiveTennis.Tournaments.Gateway.Models.Avenue;

using System.ComponentModel.DataAnnotations;

public record AvenueInputModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Location { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    [Required]
    public string Details { get; set; }
    
    public List<CourtsInfo> Courts { get; set; }
}