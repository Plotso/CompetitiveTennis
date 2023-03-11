namespace CompetitiveTennis.Tournaments.Gateway.Models;

using Avenue;
using Tournament;

public record FullSearchOutputModel(SearchOutputModel<AvenueOutputModel> Avenues, SearchOutputModel<TournamentOutputModel> Tournaments);