namespace CompetitiveTennis.Tournaments.Contracts.Match;

using Data.Models.Enums;

public record MatchResultsOutputModel(IEnumerable<SetResultOutput> setResults);

public record SetResultOutput(sbyte SetNumber, MatchOutcome Winner, ushort HomeSideGamesWon, ushort AwaySideGamesWon);