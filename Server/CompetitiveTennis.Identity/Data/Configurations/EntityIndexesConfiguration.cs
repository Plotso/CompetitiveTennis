namespace CompetitiveTennis.Identity.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Models;

internal class EntityIndexesConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        // UniqueID indexes
        var authorEntities = modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType != null && typeof(User).IsAssignableFrom(e.ClrType));
        foreach (var entity in authorEntities)
        {
            modelBuilder
                .Entity(entity.ClrType)
                .HasIndex(nameof(User.UserName))
                .IsUnique();
        }
    }
}