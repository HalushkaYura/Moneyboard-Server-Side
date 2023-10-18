using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Entities.RoleEntity
{
    public class Role :IBaseEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RolePoints { get; set; }
        public DateTime CreateDate { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<UserProject> UserProjects { get; set;}
        public string FormattedCreateDate
        {
            get { return CreateDate.ToString("dd/MM/yy"); }
        }
    }
}
