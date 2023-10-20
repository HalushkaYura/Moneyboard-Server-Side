using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectDetailsDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double BaseSalary { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime SalaryDate { get; set; }
        public int ProjectPointPercent { get; set; }

        public List<ProjectMemberDTO> Members { get; set; }
    }
}
