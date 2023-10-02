using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Interfaces;
using Moneyboard.Core.Interfaces.Repositor;

namespace Moneyboard.Infrastructure.Date.Repositories
{
    public class BaseRepositor<TEntity> : IRepositor<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly MoneyboardDb _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        public BaseRepositor(MoneyboardDb dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();

        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByKeyAsync<TKey>(TKey key)
        {
            return await _dbSet.FindAsync(key);
        }

        public async Task<TEntity> GetByPairOfKeysAsync<TFirstKey, TSecondKey>
            (TFirstKey firstKey, TSecondKey secondKey)
        {
            return await _dbSet.FindAsync(firstKey, secondKey);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => _dbContext.Entry(entity).State = EntityState.Modified);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }
        /*public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbSet.RemoveRange(entities));
        }*/

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }
    }
}
