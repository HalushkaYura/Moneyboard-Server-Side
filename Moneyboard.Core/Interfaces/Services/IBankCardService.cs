using Moneyboard.Core.DTO.BankCardDTO;
using Moneyboard.Core.DTO.ProjectDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IBankCardService
    {
        Task<BankCardInfoDTO> InfoBankCardAsync(int projectId, string userId);
        Task EditBankCardDateAsync(BankCardEditDTO banckCardEditDTO, int projectId, string userId);

    }
}
