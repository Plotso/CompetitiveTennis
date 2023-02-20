namespace CompetitiveTennis.Data.Extensions;

using System.Reflection;
using Configuration;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

public static class ModelBuilderExtensions
{
    public static void ApplyGlobalDbConfigurationsForAssembly(this ModelBuilder modelBuilder, Assembly assembly)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
        IndexConfiguration.ConfigureIsDeleted(modelBuilder);
        IndexConfiguration.ConfigureIsTestEntity(modelBuilder);
        
        var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
        
        // Set global query filter for not deleted entities only
        var deletableEntityTypes = entityTypes
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
            method.Invoke(null, new object[] { modelBuilder });
        }
        
        // Set global query filter for entities that are marked as IsTest = false
        var testEntities = entityTypes
            .Where(et => et.ClrType != null && typeof(ITestEntity).IsAssignableFrom(et.ClrType));
        foreach (var testEntityType in testEntities)
        {
            var method = SetIsTestQueryFilterMethod.MakeGenericMethod(testEntityType.ClrType);
            method.Invoke(null, new object[] { modelBuilder });
        }
        
        // Disable cascade delete
        var foreignKeys = entityTypes
            .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
        foreach (var foreignKey in foreignKeys)
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
    
    private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
        typeof(BaseDbContext).GetMethod(
            nameof(SetIsDeletedQueryFilter),
            BindingFlags.NonPublic | BindingFlags.Static);
    
    private static readonly MethodInfo SetIsTestQueryFilterMethod =
        typeof(BaseDbContext).GetMethod(
            nameof(SetIsTestQueryFilter),
            BindingFlags.NonPublic | BindingFlags.Static);

    private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
        where T : class, IDeletableEntity 
        => builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);

    private static void SetIsTestQueryFilter<T>(ModelBuilder builder)
        where T : class, ITestEntity 
        => builder.Entity<T>().HasQueryFilter(e => !e.IsTest);
}