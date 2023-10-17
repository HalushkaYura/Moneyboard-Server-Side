using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Interfaces.Repository;

namespace Moneyboard.Infrastructure.Data.Repositories
{
   public class BankCardRepository : IBankCardRepository
    {
       protected readonly MoneyboardDb _dbContext;
        protected readonly DbSet<BankCard> _dbSet;

        public BankCardRepository(MoneyboardDb dbContext, DbSet<BankCard> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }
        public async Task<BankCard> GetBankCardByProjectIdAsync(int projectId)
        {
            // Знаходимо проект з включеною інформацією про банківську картку
            var project = _dbContext.Project
                .Include(p => p.BankCard)
                .FirstOrDefault(p => p.ProjectId == projectId);

            if (project != null)
            {
                return project.BankCard;
            }

            return null;
        }

        public async Task<BankCard> GetByCardNumberAsync(string cardNumber)
        {

            var bankCardEntity = await _dbSet.SingleOrDefaultAsync(x => ((BankCard)(object)x).CardNumber == cardNumber);
            return bankCardEntity as BankCard;
       
        }
    }
}
