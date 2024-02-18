namespace CompetitiveTennis.Tournaments.Extensions;

using System.Reflection;
using CompetitiveTennis.Data.Configuration;
using CompetitiveTennis.Data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void ApplyGlobalDbConfigurationsForAssembly(
        this ModelBuilder modelBuilder,
        Assembly assembly,
        MethodInfo IsDeletedQueryFilterMethod,
        MethodInfo IsTestQueryFilterMethod)
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
            var method = IsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
            method.Invoke(null, new object[] { modelBuilder });
        }
        
        // Set global query filter for entities that are marked as IsTest = false
        var testEntities = entityTypes
            .Where(et => et.ClrType != null && typeof(ITestEntity).IsAssignableFrom(et.ClrType));
        foreach (var testEntityType in testEntities)
        {
            var method = IsTestQueryFilterMethod.MakeGenericMethod(testEntityType.ClrType);
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
}