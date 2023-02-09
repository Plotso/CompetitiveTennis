namespace CompetitiveTennis.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

public class IndexConfiguration
{
    public static void ConfigureIsDeleted(ModelBuilder modelBuilder)
    {
        // IDeletableEntity.IsDeleted index
        var deletableEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
        }
    }
    public static void ConfigureIsTestEntity(ModelBuilder modelBuilder)
    {
        // ITestEntity.IsTest index
        var testableEntities = modelBuilder.Model
            .GetEntityTypes()
            .Where(et => et.ClrType != null && typeof(ITestEntity).IsAssignableFrom(et.ClrType));
        foreach (var testableEntity in testableEntities)
        {
            modelBuilder.Entity(testableEntity.ClrType).HasIndex(nameof(ITestEntity.IsTest));
        }
    }
}