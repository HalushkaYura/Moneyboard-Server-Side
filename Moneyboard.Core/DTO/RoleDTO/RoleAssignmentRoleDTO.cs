﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.RoleDTO
{
    public class RoleAssignmentRoleDTO
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public int ProjectId { get; set; }
    }
}
