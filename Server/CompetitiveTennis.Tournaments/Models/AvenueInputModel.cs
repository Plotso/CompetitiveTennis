namespace CompetitiveTennis.Tournaments.Models;

using static Data.DataConstants.Avenues;
using System.ComponentModel.DataAnnotations;

public record AvenueInputModel
{
    [Required]
    [MinLength(MinNameLength)]
    [MaxLength(MaxNameLength)]
    public string Name { get; set; }
    
    [Required]
    [MinLength(MinLocationLength)]
    [MaxLength(MaxLocationLength)]
    public string Location { get; set; }
    
    [Required]
    [MaxLength(MaxDetailsLength)]
    public string Details { get; set; }
    
    public List<CourtsInfo> Courts { get; set; }
}