namespace CompetitiveTennis.Tournaments.Models.Avenue;

using System.ComponentModel.DataAnnotations;
using static Data.DataConstants.Avenues;

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
    [MinLength(MinCityCountryLength)]
    [MaxLength(MaxLocationLength)]
    public string City { get; set; }
    
    [Required]
    [MinLength(MinCityCountryLength)]
    [MaxLength(MaxLocationLength)]
    public string Country { get; set; }
    
    [Required]
    [MaxLength(MaxDetailsLength)]
    public string Details { get; set; }
    
    public List<CourtsInfo> Courts { get; set; }
}