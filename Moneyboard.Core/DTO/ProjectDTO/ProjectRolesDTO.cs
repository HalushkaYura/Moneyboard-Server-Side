using Moneyboard.Core.DTO.RoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectRolesDTO
    {
        public int ProjectId { get; set; }
        public List<RoleEditDTO> Roles { get; set; }
        public int ProjectPoinPercent { get; set; }
    }
}
