namespace CompetitiveTennis.Tournaments.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public static class QueryableExtensions
{
    public static IQueryable<Tournament> EnrichTournamentQueryData(this IQueryable<Tournament> tournaments)
        => tournaments
            .Include(t => t.Avenue)
            .Include(t => t.Organiser)
            .Include(t => t.Participants)
            .ThenInclude(p => p.Players)
            .ThenInclude(pp => pp.Account);
    public static IQueryable<Tournament> EnrichTournamentQueryForDrawGeneration(this IQueryable<Tournament> tournaments) 
        => tournaments
            .Include(t => t.Participants)
            .ThenInclude(p => p.Players)
            .ThenInclude(pp => pp.Account);
}