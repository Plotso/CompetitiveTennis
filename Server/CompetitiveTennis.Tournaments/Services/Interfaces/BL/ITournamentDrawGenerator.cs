namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Models;

public interface ITournamentDrawGenerator
{
    Task<bool> GenerateTournamentDraw(FullTournamentOutputModel fullTournament);
}