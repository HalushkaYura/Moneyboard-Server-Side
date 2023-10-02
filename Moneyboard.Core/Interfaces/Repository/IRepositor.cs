namespace Moneyboard.Core.Interfaces.Repositor
{
    public interface IRepositor<TEntity> where TEntity : IBaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByKeyAsync<TKey>(TKey key);
        Task<TEntity> GetByPairOfKeysAsync<TKey, TKey1>(TKey key, TKey1 key1);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(List<TEntity> entities);
        //Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        //IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
        //Task<int> SqlQuery(string sqlQuery);

    }
}
