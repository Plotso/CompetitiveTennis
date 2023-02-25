namespace CompetitiveTennis.Tournaments.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Models;

internal class EntityIndexesConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        // UniqueID indexes
        var customerEntities = modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType != null && typeof(Account).IsAssignableFrom(e.ClrType));
        foreach (var entity in customerEntities)
        {
            modelBuilder
                .Entity(entity.ClrType)
                .HasIndex(nameof(Account.UserId))
                .IsUnique();
            
            modelBuilder
                .Entity(entity.ClrType)
                .HasIndex(nameof(Account.Username))
                .IsUnique();
        }
    }
}