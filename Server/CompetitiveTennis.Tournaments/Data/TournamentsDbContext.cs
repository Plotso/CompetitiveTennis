namespace CompetitiveTennis.Tournaments.Data;

using CompetitiveTennis.Data.Extensions;
using CompetitiveTennis.Data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using static Constants.CustomDbTypes;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Avenue> Avenues { get; set; }
    
    public DbSet<Tournament> Tournaments { get; set; }
    
    public DbSet<Participant> Participants { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Match> Matches { get; set; }
    
    public DbSet<ParticipantMatch> ParticipantMatches { get; set; }
    
    public DbSet<Score> Scores { get; set; }
    
    public override int SaveChanges() => SaveChanges(true);

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInfoRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<Surface>(name: SurfaceEnum);
        modelBuilder.HasPostgresEnum<TournamentType>(name: TournamentTypeEnum);
        modelBuilder.HasPostgresEnum<EventStatus>(name: EventStatusEnum);
        modelBuilder.HasPostgresEnum<MatchOutcome>(name: MatchOutcomeEnum);
        modelBuilder.ApplyGlobalDbConfigurationsForAssembly(GetType().Assembly);
    }

    private void ApplyAuditInfoRules()
    {
        var changedEntries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is IAuditInfo &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in changedEntries)
        {
            var entity = (IAuditInfo)entry.Entity;
            if (entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}