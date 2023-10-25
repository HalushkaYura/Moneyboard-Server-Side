using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectCalculationDTO
    {
        public double BankCardMoney { get; set; }
        public double TotalPayment { get; set; }
        public bool IsEnough { get; set; }
    }
}
