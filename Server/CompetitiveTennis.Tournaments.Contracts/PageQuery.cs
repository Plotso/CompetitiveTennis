namespace CompetitiveTennis.Tournaments.Contracts;

public record PageQuery(int Page = 1, int ItemsPerPage = 25);