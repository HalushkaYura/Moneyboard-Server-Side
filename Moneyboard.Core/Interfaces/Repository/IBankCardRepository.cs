using Moneyboard.Core.Entities.BankCardEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Repository
{
   public interface IBankCardRepository
    {
        Task<BankCard> GetBankCardByProjectIdAsync(int projectId);
        Task<BankCard> GetByCardNumberAsync(string cardNumber);

    }
}
