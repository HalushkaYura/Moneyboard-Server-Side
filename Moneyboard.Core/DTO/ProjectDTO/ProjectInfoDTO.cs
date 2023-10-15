using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectInfoDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double BaseSalary { get; set; }
        public DateTime SalaryDate { get; set; }
        public List<Role> RoleInfo { get; set; }
    }
}
