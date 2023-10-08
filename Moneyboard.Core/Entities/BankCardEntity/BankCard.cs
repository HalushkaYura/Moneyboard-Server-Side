using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Interfaces;

namespace Moneyboard.Core.Entities.BankCardEntity
{
    public class BankCard : IBaseEntity
    {
        public int BankCardId { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CardVerificationValue { get; set; }

        public double Money { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
