using FluentValidation;
using Moneyboard.Core.DTO.ProjectDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Validation
{
    public class PersonalPointValidator : AbstractValidator<PersonalPointDTO>
    {
        public PersonalPointValidator()
        {
            RuleFor(dto => dto.PersonalPoint)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }
    }
}
