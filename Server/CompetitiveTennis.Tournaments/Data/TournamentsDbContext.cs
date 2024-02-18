namespace CompetitiveTennis.Tournaments.Data;

using System.Reflection;
using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Data.Models.Interfaces;
using Configurations;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using static DataConstants.CustomDbTypes;

public class TournamentsDbContext : DbContext
{
    public TournamentsDbContext(DbContextOptions<TournamentsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Avenue> Avenues { get; set; }
    
    public DbSet<Tournament> Tournaments { get; set; }
    
    public DbSet<Participant> Participants { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Match> Matches { get; set; }
    
    public DbSet<ParticipantMatch> ParticipantMatches { get; set; }
    public DbSet<AccountParticipant> AccountParticipants { get; set; }
    
    public DbSet<MatchPeriod> MatchPeriods { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<MatchFlow> MatchFlows { get; set; }
    
    public override int SaveChanges() => SaveChanges(true);

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInfoRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => SaveChangesAsync(true, cancellationToken);

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditInfoRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Surface>(SurfaceEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TournamentType>(TournamentTypeEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<TournamentStage>(TournamentStageEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<EventStatus>(EventStatusEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<MatchOutcome>(MatchOutcomeEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<EventActor>(EventActorEnum);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<OutcomeCondition>(OutcomeConditionEnum);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresEnum<Surface>(name: SurfaceEnum);
        modelBuilder.HasPostgresEnum<TournamentType>(name: TournamentTypeEnum);
        modelBuilder.HasPostgresEnum<TournamentStage>(name: TournamentStageEnum);
        modelBuilder.HasPostgresEnum<EventStatus>(name: EventStatusEnum);
        modelBuilder.HasPostgresEnum<MatchOutcome>(name: MatchOutcomeEnum);
        modelBuilder.HasPostgresEnum<OutcomeCondition>(name: OutcomeConditionEnum);
        modelBuilder.HasPostgresEnum<EventActor>(name: EventActorEnum);
        modelBuilder.ApplyGlobalDbConfigurationsForAssembly(GetType().Assembly, SetIsDeletedQueryFilterMethod, SetIsTestQueryFilterMethod);
        EntityIndexesConfiguration.Configure(modelBuilder);
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
    
    private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
        typeof(TournamentsDbContext).GetMethod(
            nameof(SetIsDeletedQueryFilter),
            BindingFlags.NonPublic | BindingFlags.Static);
    
    private static readonly MethodInfo SetIsTestQueryFilterMethod =
        typeof(TournamentsDbContext).GetMethod(
            nameof(SetIsTestQueryFilter),
            BindingFlags.NonPublic | BindingFlags.Static);

    private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
        where T : class, IDeletableEntity 
        => builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);

    private static void SetIsTestQueryFilter<T>(ModelBuilder builder)
        where T : class, ITestEntity 
        => builder.Entity<T>().HasQueryFilter(e => !e.IsTest);
}