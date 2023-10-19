using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.BankCardDTO
{
    public class BankCardEditDTO
    {
        public string CardNumber { get; set; }
        public string CardVerificationValue { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
