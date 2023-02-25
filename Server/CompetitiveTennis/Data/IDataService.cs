namespace CompetitiveTennis.Data;

public interface IDataService<in TEntity>
    where TEntity : class
{
    Task SaveAsync(TEntity entity);
}