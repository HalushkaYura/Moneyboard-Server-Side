using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectEditDTO
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public DateTime SalaryDate { get; set; }
        public string NumberCard { get; set; }
        public string CVV { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Money { get; set; }
        public CurrencyType SelectedCurrency { get; set; }
        public int ProjectPoinPercent { get; set; }
    }
}
