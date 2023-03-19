namespace CompetitiveTennis.Data;

using Microsoft.EntityFrameworkCore;

public abstract class DataService<TEntity> : IDataService<TEntity>
    where TEntity : class
{
    protected DataService(DbContext db) => Data = db;

    protected DbContext Data { get; }

    protected IQueryable<TEntity> All() => Data.Set<TEntity>();
    protected IQueryable<TEntity> AllAsNoTracking() => Data.Set<TEntity>().AsNoTracking();

    protected IQueryable<TEntity> AllIgnoreQueryFilters() => All().IgnoreQueryFilters();

    protected IQueryable<TEntity> AllAsNoTrackingIgnoreQueryFilters() => AllAsNoTracking().IgnoreQueryFilters();

    public async Task SaveAsync(TEntity entity)
    {
        Data.Update(entity);

        await Data.SaveChangesAsync();
    }
}