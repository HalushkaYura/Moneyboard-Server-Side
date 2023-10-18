using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Interfaces;

namespace Moneyboard.Core.Entities.ProjectEntity
{
    public class Project : IBaseEntity
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double BaseSalary { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime SalaryDate { get; set; }
        public int ProjectPoinPercent { get; set; }

        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<Role> Roles { get; set; }

        public int BankCardId { get; set; }
        public BankCard BankCard { get; set; }

    }
}
