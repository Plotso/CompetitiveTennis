namespace CompetitiveTennis.Tournaments.Gateway.Models.Tournament;

using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;

public record TournamentInputModel
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Rules { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public TournamentType Type { get; set; }
    
    [Required]
    public Surface Surface { get; set; }
    
    public decimal? EntryFee { get; set; }
    
    public decimal? Prize { get; set; }
    
    [Required]
    [Range(1, short.MaxValue)]
    public short CourtsAvailable { get; set; }
    
    [Required]
    [Range(2, short.MaxValue)]
    public short MinParticipants { get; set; }
    
    [Required]
    [Range(2, short.MaxValue)]
    public short MaxParticipants { get; set; }

    public short? MatchWonPoints { get; set; }
    public short? SetWonPoints { get; set; }
    public short? GameWonPoints { get; set; }
    
    [Required]
    public bool IsIndoor { get; set; }
    
    [Required]
    public bool IsLeague { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int AvenueId { get; set; }
}