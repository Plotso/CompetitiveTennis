namespace CompetitiveTennis.Tournaments.Services.Interfaces.BL;

using Models.TournamentDrawGenerator;

public interface IMatchesGenerator
{
    List<MatchGeneratorOutput> GenerateMatches(List<Seed> seeds);
}