using Ardalis.Specification;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using System.Linq.Expressions;

namespace Moneyboard.Core.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByKeyAsync<TKey>(TKey key);
        Task<TEntity> GetByPairOfKeysAsync<TKey, TKey1>(TKey key, TKey1 key1);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(List<TEntity> entities);
        Task<TEntity> GetFirstBySpecAsync(ISpecification<TEntity> specification);
        Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);
        Task<TEntity> GetEntityAsync(
              Expression<Func<TEntity, bool>> filter = null,
              string includeProperties = null);
    

        //Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        //IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
        //Task<int> SqlQuery(string sqlQuery);

    }
}
