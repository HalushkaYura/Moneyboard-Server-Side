using Moneyboard.Core.DTO.RoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task CreateNewRoleAsync(RoleCreateDTO roleCreateDTO, int projectId);

    }
}
