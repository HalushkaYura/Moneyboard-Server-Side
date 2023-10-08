﻿using FluentValidation;
using Moneyboard.Core.DTO.RoleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Validation
{
    public class RoleCreateValidation : AbstractValidator<RoleCreateDTO>
    {
        public RoleCreateValidation()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .MaximumLength(255);
            RuleFor(dto => dto.RolePoints)
                .NotEmpty()
                .LessThanOrEqualTo(0);
        }
    }
}
