namespace CompetitiveTennis.Data;

using Microsoft.EntityFrameworkCore;
using Models.Interfaces;

public abstract class DeletableDataService<TEntity> : DataService<TEntity>
    where TEntity : class, IDeletableEntity
{
    protected DeletableDataService(DbContext db) : base(db){}

    protected void HardDelete(TEntity entity) => Data.Remove(entity);

    protected async Task RestoreDeleted(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedOn = null;
        await SaveAsync(entity);
    }

    protected async Task Delete(TEntity entity, string userid)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
        entity.DeletedBy = userid;
        await SaveAsync(entity);
    }
}