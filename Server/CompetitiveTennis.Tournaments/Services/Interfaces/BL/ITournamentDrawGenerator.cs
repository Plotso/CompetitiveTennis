namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Models.Tournament;

public interface ITournamentDrawGenerator
{
    Task<bool> GenerateTournamentDraw(TournamentOutputModel tournament);
}