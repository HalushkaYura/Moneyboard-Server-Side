using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectCreateDTO
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public DateTime SalaryDate { get; set; }
        public string NumberCard { get; set; }
        public int CVV { get; set; }
        public DateTime MyPropertyExpirationDate { get; set; }
        public double Maney { get; set; }
        public Currency SelectedCurrency { get; set; }
    }
}
