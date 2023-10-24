using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.ProjectDTO
{
    public class ProjectMemberDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RolePoints { get; set; }
        public int PersonalPoints { get; set; }
        public bool? IsOwner { get; set; }
        public bool? IsDefolt { get; set; }
        public double UserPayment { get; set; }
        public double RolePayment { get; set; }
        public double PersonelPayment { get; set; }
    }
}
