using FluentValidation;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Validation
{
    public class RoleEditValidation : AbstractValidator<RoleEditDTO>
    {
        public RoleEditValidation()
        {
            RuleFor(dto => dto.RoleName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(dto => dto.RolePoints)
                .GreaterThanOrEqualTo(0);


        }
    }
}
