using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectCreateDTO
    {
        public string Name { get; set; }
        public double BaseSalary { get; set; }
        //public DateTime SalaryDate { get; set; }
        public int SalaryDay { get; set; }
        public string CardNumber { get; set; }
        public string CardVerificationValue { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Money { get; set; }
        public string Currency { get; set; }
        //public int ProjectPoinPercent { get; set; }
    }
}
