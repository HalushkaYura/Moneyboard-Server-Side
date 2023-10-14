using Ardalis.Specification;
using Moneyboard.Core.Entities.BankCardEntity;

namespace Moneyboard.Core.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByKeyAsync<TKey>(TKey key);
        Task<BankCard> GetByCardNumberAsync(string cardNumber);
        Task<TEntity> GetByPairOfKeysAsync<TKey, TKey1>(TKey key, TKey1 key1);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(List<TEntity> entities);
        Task<TEntity> GetFirstBySpecAsync(ISpecification<TEntity> specification);
        Task<BankCard> GetBankCardByProjectIdAsync(int projectId);
        //Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        //IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
        //Task<int> SqlQuery(string sqlQuery);

    }
}
