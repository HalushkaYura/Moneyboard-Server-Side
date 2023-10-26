using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectEditDTO
    {
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        public int SalaryDay { get; set; }
        public string Currency { get; set; }
    }
}
