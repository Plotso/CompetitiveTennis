namespace CompetitiveTennis.Tournaments.Models.Tournament;

using System.ComponentModel.DataAnnotations;

public record TournamentChangeAvenueInputModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int TournamentId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int NewAvenueId { get; set; }
};