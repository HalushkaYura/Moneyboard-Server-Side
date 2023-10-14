using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Entities.UserProjectEntity
{
    public class UserProject : IBaseEntity
    {
        
        public int UserProjectId { get; set; }
        public bool IsOwner { get; set; }
        public DateTime MemberDate { get; set; }
        public int PersonalPoints { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
