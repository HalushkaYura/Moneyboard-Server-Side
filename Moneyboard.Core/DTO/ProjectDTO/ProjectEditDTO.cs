using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectEditDTO
    {
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        public int SalaryDay { get; set; }
        public CurrencyType Currency { get; set; }
        public int ProjectPoinPercent { get; set; }

    }
}
