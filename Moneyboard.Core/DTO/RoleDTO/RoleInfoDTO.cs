﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.DTO.RoleDTO
{
    public class RoleInfoDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RolePoints { get; set; }
        public bool IsDefolt { get; set; }
    }
}
