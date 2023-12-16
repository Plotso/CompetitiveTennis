namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Contracts.Tournament;

public interface ITournamentDrawGenerator
{
    Task<bool> GenerateTournamentDraw(TournamentOutputModel tournament);
}